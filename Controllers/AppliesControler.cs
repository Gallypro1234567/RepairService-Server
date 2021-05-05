using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using WorkAppReactAPI.Assets;
using WorkAppReactAPI.Configuration;
using WorkAppReactAPI.Data.Interface;
using WorkAppReactAPI.Dtos.Requests;
using WorkAppReactAPI.Models.Responses;

namespace WorkAppReactAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AppliesController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        private readonly IApplyToPostRepo _repository;
        public AppliesController(IWebHostEnvironment hostEnvironment, IApplyToPostRepo repository)
        {
            _hostingEnvironment = hostEnvironment;
            _repository = repository;

        }

        [Authorize]
        [HttpGet("checkby")]
        public async Task<ActionResult<DynamicResult>> CheckApplyofPosts([FromQuery] string postcode, [FromHeader] HeaderParamaters header)
        {

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
            var result = await _repository.checkApplytoPostbyWorkerPhone(postcode, auth);
            return Ok(result);
        }
        [Authorize]
        [HttpGet("getbycustomer")]
        public async Task<ActionResult<DynamicResult>> getApplyofPosts([FromQuery] string postcode)
        {

            var result = await _repository.getApplytoPostbyCode(postcode);
            return Ok(result);
        }
        [Authorize]
        [HttpGet("getbyworker")]
        public async Task<ActionResult<DynamicResult>> getApplyPostby([FromQuery] string phone)
        {

            var result = await _repository.getApplytoPostbyWorkerPhone(phone);
            return Ok(result);
        }

        [Authorize]
        [HttpPost]

        [Route("addbycustomer")]
        public async Task<ActionResult<DynamicResult>> addPost([FromForm] ApplyToPostUpdate model, [FromHeader] HeaderParamaters header)
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
                result = await _repository.AddApplytoPost(model, auth);
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

        [Route("updatebycustomer")]
        public async Task<ActionResult<DynamicResult>> updatePostbyCustomer([FromForm] ApplyToPostUpdate model, [FromHeader] HeaderParamaters header)
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
                result = await _repository.UpdateApplytoPost(model, auth);
                if (result.Status == 1)
                {
                    var result1 = await _repository.customerAcceptPostApply(model, auth); 
                    return Ok(result1);
                }
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
        [Route("deletebyworker")]
        public async Task<ActionResult<DynamicResult>> deletePostById([FromForm] ApplyToPostUpdate model, [FromHeader] HeaderParamaters header)
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
                result = await _repository.DeleteApplytoPost(model, auth);
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