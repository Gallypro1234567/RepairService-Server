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
    public class PreferentialsController : ControllerBase
    {


        private readonly IPreferentialRepo _repository;

        public PreferentialsController(IPreferentialRepo repository)
        {
            _repository = repository;

        }


        [HttpGet]
        public async Task<ActionResult<DynamicResult>> getAllPreferential()
        {

            var result = await _repository.ListPreferentials();
            return result;
        }

        [HttpPost]
        [Route("add")]
        public async Task<ActionResult<DynamicResult>> AddPreferential([FromBody] PreferentialUpdate model)
        {

            var result = await _repository.AddPreferential(model);
            return result;
        }

        [HttpPost]
        [Route("update")]
        public async Task<ActionResult<DynamicResult>> UpdatePreferential([FromBody] PreferentialUpdate model)
        {

            var result = await _repository.UpdatePreferential(model);
            return result;
        }

    }
}