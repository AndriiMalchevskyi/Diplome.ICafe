using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ICafe.Application;
using ICafe.Application.Interfaces;
using ICafe.Application.Models.Filter;
using ICafe.Application.Models.Order;
using ICafe.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ICafe.API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IRepository<Order> _repository;
        private readonly IRepository<Product> _productRepository;
        private IRepository<User> _userRepo;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public OrderController(IMapper mapper, IRepository<Order> repository, IRepository<Product> productRepository,
            IRepository<User> userRepo, UserManager<User> userManager)
        {
            _mapper = mapper;
            _repository = repository;
            _productRepository = productRepository;
            _userRepo = userRepo;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Add(OrderToCreateDto orderToCreateDto)
        {
            try
            {
                var orderToCreate = _mapper.Map<Order>(orderToCreateDto);
                ///orderToCreate.Products = new List<Product>();
                var user = await _userManager.GetUserAsync(User);
                List<ProductOrder> productOrders = new List<ProductOrder>();
                foreach (var id in orderToCreateDto.ProductsIds)
                {

                    var product = await _productRepository.Get(new Product() { Id = id.Key });
                    productOrders.Add(new ProductOrder
                    {
                        ProductId = product.Id,
                        Product = product,
                        ProductCount = id.Value
                    }); ;
                }

                if (user != null)
                {
                    orderToCreate.Owner = user;
                }

                var result = await _repository.Add(orderToCreate);
                for (int i=0; i< productOrders.Count;i++)
                {
                    productOrders[i].OrderId = result.Id;
                    productOrders[i].Order = result;
                }
                result.ProductOrders = productOrders;
                result.Status = "Open";
                result = await _repository.Update(result);
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
                var orderToDelete = _mapper.Map<Order>(new Order() { Id = id });

                var result = await _repository.Delete(orderToDelete);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut]
        public async Task<IActionResult> Update(OrderToUpdateDto orderToUpdateDto)
        {
            try
            {
                var orderToDelete = _mapper.Map<Order>(orderToUpdateDto);

                var result = await _repository.Update(orderToDelete);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var orderToGet = _mapper.Map<Order>(new Order() { Id = id });

                var result = await _repository.Get(orderToGet);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] FilterDto filter)
        {
            try
            {
                var filterIns = _mapper.Map<Filter>(filter);
                var result = await _repository.Get(filterIns);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}