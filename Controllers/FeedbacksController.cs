 
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc; 
using WorkAppReactAPI.Configuration;
using WorkAppReactAPI.Data.Interface;
using WorkAppReactAPI.Dtos.Requests;
using WorkAppReactAPI.Models.Responses;

namespace WorkAppReactAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FeedBacksController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        private readonly IFeedBackRepo _repository;
        public FeedBacksController(IWebHostEnvironment hostEnvironment, IFeedBackRepo repository)
        {
            _hostingEnvironment = hostEnvironment;
            _repository = repository;

        }

        [Authorize]
        [HttpGet("getby")]
        public async Task<ActionResult<DynamicResult>> get([FromQuery] FeedbackQuery query)
        {

            var result = await _repository.getFeedBackByWofSCode(query);
            return Ok(result);
        }
        [Authorize]
        [HttpGet("getdetail")]
        public async Task<ActionResult<DynamicResult>> getdetail([FromQuery] string wofscode)
        {

            var result = await _repository.getWorkerRating(wofscode);
            return Ok(result);
        }
       
        [Authorize]
        [HttpPost]

        [Route("add")]
        public async Task<ActionResult<DynamicResult>> add([FromForm] FeedbackUpdate model, [FromHeader] HeaderParamaters header)
        {
            try
            {

                var result = new DynamicResult();

                var handler = new JwtSecurityTokenHandler();
                var tokenStr = header.Authorization.Substring("Bearer ".Length).Trim();
                var jsonToken = handler.ReadToken(tokenStr);
                var tokenS = jsonToken as JwtSecurityToken;

                var auth = new UserLogin()
                {
                    Phone = tokenS.Claims.First(claim => claim.Type == "Phone").Value,
                    Password = tokenS.Claims.First(claim => claim.Type == "Password").Value,
                    isCustomer = bool.Parse(tokenS.Claims.First(claim => claim.Type == "isCustomer").Value),
                    Role = int.Parse(tokenS.Claims.First(claim => claim.Type == "Role").Value),
                };
                result = await _repository.AddFeedBack(model, auth);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new RegistrationResponse
                {
                    Errors = new List<string>(){
                            ex.Message
                        },
                    Success = false
                });
            }

        } 
         
        [Authorize]
        [HttpPost]
        [Route("deletebycustomer")]
        public async Task<ActionResult<DynamicResult>> deletebycode([FromQuery] string code, [FromHeader] HeaderParamaters header)
        {

            try
            {
                var result = new DynamicResult();

                var handler = new JwtSecurityTokenHandler();
                var tokenStr = header.Authorization.Substring("Bearer ".Length).Trim();
                var jsonToken = handler.ReadToken(tokenStr);
                var tokenS = jsonToken as JwtSecurityToken;

                var auth = new UserLogin()
                {
                    Phone = tokenS.Claims.First(claim => claim.Type == "Phone").Value,
                    Password = tokenS.Claims.First(claim => claim.Type == "Password").Value,
                    isCustomer = bool.Parse(tokenS.Claims.First(claim => claim.Type == "isCustomer").Value),
                    Role = int.Parse(tokenS.Claims.First(claim => claim.Type == "Role").Value),
                };
                result = await _repository.DeleteFeedBack(code, auth);
                return Ok(result);
            }
            catch (System.Exception ex)
            {

                return BadRequest(new RegistrationResponse
                {
                    Errors = new List<string>(){
                            ex.Message
                        },
                    Success = false
                });
            }

        }
        
    }
}