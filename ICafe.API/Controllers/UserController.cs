using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ICafe.Application;
using ICafe.Application.Interfaces;
using ICafe.Application.Models.Filter;
using ICafe.Application.Models.User;
using ICafe.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ICafe.API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer", Policy = "RequireAdministratorRoles")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private IRepository<User> _userRepo;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _singInManager;

        public UserController(IMapper mapper,
            IRepository<User> userRepo,
            UserManager<User> userManager,
            SignInManager<User> singInManager)
        {
            _userRepo = userRepo;
            _userManager = userManager;
            _singInManager = singInManager;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers([FromQuery]FilterDto filter)
        {
            var filterIns = _mapper.Map<Filter>(filter);
            var list = await _userRepo.Get(filterIns);
            var listToReturn = _mapper.Map<UserForDetailedDto[]>(list);
            return Ok(listToReturn);
        }

        [AllowAnonymous]
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUser([FromRoute] string id)
        {
            UserForDetailedDto user = null;
            try
            {
                var userFromBase = await _userRepo.Get(new User() { Id = Convert.ToInt32(id) });
                user = _mapper.Map<UserForDetailedDto>(userFromBase);
            }
            catch(Exception ex)
            {
                BadRequest(ex.Message);
            }

            return Ok(user);
        }

        public class RoleChanger
        {
            public string id { get; set; }
            public string role { get; set; }
        }

        [HttpPut("user")]
        public async Task<IActionResult> AddRoleToUser([FromBody] RoleChanger role)
        {
            try
            {
                var userFromBase = await _userManager.FindByIdAsync(role.id);
                var res = await _userManager.AddToRoleAsync(userFromBase, role.role);
                if (res.Succeeded)
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                BadRequest(ex.Message);
            }

            return BadRequest();
        }
    }
}