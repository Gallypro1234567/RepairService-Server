using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkAppReactAPI.Configuration; 
using WorkAppReactAPI.Data.Interface;

namespace WorkAppReactAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NewsController : ControllerBase
    {


        private readonly IServiceRepo _repository;

        public NewsController(IServiceRepo repository)
        {
            _repository = repository;

        }

         
        [HttpGet]
        public async Task<ActionResult<DynamicResult>> getAllNews()
        {

            var list = await _repository.getListService();
            return list;
        }
        
    }
}