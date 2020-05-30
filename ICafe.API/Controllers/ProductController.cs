using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ICafe.Application.Interfaces;
using ICafe.Domain.Entities;
using ICafe.Application.Models.Product;
using Microsoft.AspNetCore.Authorization;
using ICafe.Application;
using ICafe.Application.Models.Filter;

namespace ICafe.API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IRepository<Product> _repository;
        private readonly IMapper _mapper;

        public ProductController(IMapper mapper, IRepository<Product> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }


        [HttpPost]
        public async Task<IActionResult> Add(ProductToCreateDto productToCreateDto)
        {
            try
            {
                var productToCreate = _mapper.Map<Product>(productToCreateDto);

                var result = await _repository.Add(productToCreate);
            
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var productToDelete = _mapper.Map<Product>(new Product() { Id = id });

                var result = await _repository.Delete(productToDelete);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        
        [HttpPut]
        public async Task<IActionResult> Update(ProductToUpdateDto productToUpdateDto)
        {
            try
            {
                var productToDelete = _mapper.Map<Product>(productToUpdateDto);

                var result = await _repository.Update(productToDelete);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var productToGet = _mapper.Map<Product>(new Product(){ Id = id });

                var result = _mapper.Map<ProductToDetailDto>(await _repository.Get(productToGet));

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] FilterDto filter)
        {
            try
            {
                var filterIns = _mapper.Map<Filter>(filter);
                var result = _mapper.Map<ProductToDetailDto[]>(await _repository.Get(filterIns));

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}