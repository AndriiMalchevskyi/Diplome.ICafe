using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using System.Threading.Tasks;
using ICafe.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using ICafe.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using ICafe.Application.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using ICafe.API.Helpers;

namespace ICafe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _singInManager;

        public AuthController(IMapper mapper,
            IConfiguration config,
            UserManager<User> userManager,
            SignInManager<User> singInManager)
        {
            _userManager = userManager;
            _singInManager = singInManager;
            _config = config;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            var userByEmail = await GetUserByLogin(userForRegisterDto.Email);
            var userByName = await GetUserByLogin(userForRegisterDto.UserName);
            if (userByEmail != null)
            {
                return BadRequest("Email "+ userForRegisterDto.Email+" already exist");
            }
            else if (userByName != null)
            {
                return BadRequest("User name  " + userForRegisterDto.UserName + " already exist");
            }

            var userToCreate = _mapper.Map<User>(userForRegisterDto);

            var result = await _userManager.CreateAsync(userToCreate, userForRegisterDto.Password);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(userToCreate.Email);
                await _userManager.AddToRolesAsync(user, new[] { "visitor" });
                
                
                var ctoken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "Auth",
                        new { userId = user.Id, code = code },
                        protocol: HttpContext.Request.Scheme);
                    EmailService emailService = new EmailService();
                    await emailService.SendEmailAsync(user.Email, "Confirm your email address",
                        $"Confirm your email please: <a href='{callbackUrl}'>link</a>");

                return Ok(new { mess = "Confirm your email, please" });
            }

            return BadRequest(result.Errors);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return BadRequest();
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest();
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return Ok("Confirmed");
            else
                return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            try
            {
                var user = await GetUserByLogin(userForLoginDto.Login);

                var result = await _singInManager
                    .CheckPasswordSignInAsync(user, userForLoginDto.Password, false);

                if (result.Succeeded)
                {
                    user.LastActive = DateTime.Now;
                    var userToReturn = _mapper.Map<UserForListDto>(user);
                    await _userManager.UpdateAsync(user);
                    return Ok(new
                    {
                        token = GenerateJwtToken(user).Result,
                        user = userToReturn
                    });
                }
            }
            catch(Exception ex)
            {
                return Unauthorized(ex.Message);
            }

            return Unauthorized();
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("password/reset")]
        public async Task<IActionResult> ResetPassword(UserPasswordToResetDto passwordToReset)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var result = await _userManager.ChangePasswordAsync(user, passwordToReset.oldPassword, passwordToReset.newPassword);

                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        private async Task<User> GetUserByLogin(string login)
        {
            var user = await _userManager.FindByEmailAsync(login);

            if (user == null)
            {
                user = await _userManager.FindByNameAsync(login);
            }

            return user;
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(_config.GetSection("AppSettings:Token").Value));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}