using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkAppReactAPI.Data.Interface;

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

        [HttpGet] 
        public ActionResult<DynamicResult> getAllService()
        {

            var list = _repository.getListService();
            return list;
        }
    }
}