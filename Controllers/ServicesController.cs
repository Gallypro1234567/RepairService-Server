using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkAppReactAPI.Configuration;
using WorkAppReactAPI.Data.Interface;
using WorkAppReactAPI.Dtos.Requests;
using WorkAppReactAPI.Models;

namespace WorkAppReactAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ServicesController : ControllerBase
    {


        private readonly IServiceRepo _repository;

        public ServicesController(IServiceRepo repository)
        {
            _repository = repository;

        }


        [Authorize]
        [HttpGet]
        public async Task<ActionResult<DynamicResult>> getAllService()
        {

            var list = await _repository.getListService();
            return list;
        }

        [Authorize]
        [HttpPost]
        [Route("add")]
        public async Task<ActionResult<DynamicResult>> addService([FromBody] ServiceUpdate model)
        {
         
            var list = await _repository.AddService(model);
            return list;
        }
        [Authorize]
        [HttpPost]
        [Route("update")]
        public async Task<ActionResult<DynamicResult>> updateService([FromBody] ServiceUpdate model)
        {

            var list = await _repository.UpdateService(model);
            return list;
        }
        [Authorize]
        [HttpPost]
        [Route("delete")]
        public async Task<ActionResult<DynamicResult>> deleteServiceById([FromBody] ServiceDrop model)
        {

            var list = await _repository.DeleteService(model);
            return list;
        }
    }
}