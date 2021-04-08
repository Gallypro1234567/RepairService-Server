using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkAppReactAPI.Configuration;
using WorkAppReactAPI.Data.Interface;
using WorkAppReactAPI.Dtos.Requests;

namespace WorkAppReactAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepo _repository;

        public CustomersController(ICustomerRepo repository)
        {
            _repository = repository;

        }
        [HttpGet]
        [Route("getall")]
        public async Task<ActionResult<DynamicResult>> getAllCustomer()
        {
            var result = await _repository.getAllCustomer();
            return Ok(result);
        }
        [HttpGet()]
        [Route("getbyid")]
        public async Task<ActionResult<DynamicResult>> getCustomerById(string code)
        {
            
            var result = await _repository.GetCustomerByCode(code);
            return Ok(result);
        }

        [HttpGet]
        [Route("update")]
        public async Task<ActionResult<DynamicResult>> updateCustomer(UserUpdate model)
        {
            var result = await _repository.UpdateCustomer(model);
            return Ok(result);
        }

    }
}