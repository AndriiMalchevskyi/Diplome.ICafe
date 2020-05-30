using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ICafe.API.Helpers;
using ICafe.Application.Interfaces;
using ICafe.Application.Models.Photo;
using ICafe.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ICafe.API.Controllers
{
    //[Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly IRepository<Photo> _repoPhoto;
        private readonly IRepository<User> _repoUser;
        private readonly IRepository<Product> _repoProduct;

        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;
        public PhotoController(IRepository<Photo> repoPhoto, IRepository<User> repoUser, IRepository<Product> repoProduct, IMapper mapper,
            IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _repoPhoto = repoPhoto;
            _cloudinaryConfig = cloudinaryConfig;
            _repoUser = repoUser;
            _repoProduct = repoProduct;
            _mapper = mapper;

            var acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);

        }

        //[AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepo = await _repoPhoto.Get(new Photo { Id = id });

            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);

            return Ok(photo);
        }

        [HttpPost("product/{id}")]
        public async Task<IActionResult> AddPhotoForProduct(int Id, [FromForm]PhotoForCreationDto photoForCreationDto)
        {
            var productFromRepo = await _repoProduct.Get(new Product { Id = Id });

            var file = photoForCreationDto.File;

            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            photoForCreationDto.Url = uploadResult.Uri.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoForCreationDto);

            if (productFromRepo.Photo == null)

            productFromRepo.Photo = photo;

            var result = await _repoProduct.Update(productFromRepo);

            if (result != null)
            {
                return Ok();
            }

            return BadRequest("Could not add the photo");
        }

        [HttpPost("user/{id}")]
        public async Task<IActionResult> AddPhotoForUser(int Id, [FromForm]PhotoForCreationDto photoForCreationDto)
        {
            var userFromRepo = await _repoUser.Get(new User { Id = Id });

            var file = photoForCreationDto.File;

            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            photoForCreationDto.Url = uploadResult.Uri.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoForCreationDto);
            var photoRes = await _repoPhoto.Add(photo);
            if (photoRes != null)
                userFromRepo.Photo = photoRes;

            var result = await _repoUser.Update(userFromRepo);

            if (result != null)
            {
                return Ok();
            }

            return BadRequest("Could not add the photo");
        }
    }
}