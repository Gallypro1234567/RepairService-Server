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
using WorkAppReactAPI.Dtos;
using WorkAppReactAPI.Dtos.Requests;
using WorkAppReactAPI.Models;
using WorkAppReactAPI.Models.Responses;

namespace WorkAppReactAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ServicesController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        private readonly IServiceRepo _repository;

        public ServicesController(IWebHostEnvironment hostEnvironment, IServiceRepo repository)
        {
            _hostingEnvironment = hostEnvironment;
            _repository = repository;

        }


        [Authorize]
        [HttpGet]
        public async Task<ActionResult<DynamicResult>> getAllService([FromQuery] Query model)
        {

            var result = await _repository.getListService(model);
            return Ok(result);
        }
        [Authorize]
        [HttpGet("{code}")]
        public async Task<ActionResult<DynamicResult>> getAllService(string code)
        {

            var result = await _repository.getServiceDetail(code);
            return Ok(result);
        }
        [Authorize]
        [HttpPost]
        [Route("add")]
        public async Task<ActionResult<DynamicResult>> addService([FromForm] ServiceUpdate model, [FromHeader] HeaderParamaters header)
        {
            try
            {
                var result = new DynamicResult();
                var file = model.Image;
                if (file != null)
                {
                    var path = _hostingEnvironment.UploadImage(file, "\\Upload\\Images\\");
                    if (path.Length == 0)
                    {
                        return BadRequest(new DynamicResult()
                        {
                            Message = "File không hợp lệ",
                            Status = 1
                        });
                    }
                    model.ImageUrl = path;
                }


                var handler = new JwtSecurityTokenHandler();
                var tokenStr = header.Authorization.Substring("Bearer ".Length).Trim();
                var jsonToken = handler.ReadToken(tokenStr);
                var tokenS = jsonToken as JwtSecurityToken;

                var auth = new UserLogin()
                {
                    Phone = tokenS.Claims.First(claim => claim.Type == "Phone").Value,
                    Password = tokenS.Claims.First(claim => claim.Type == "Password").Value,
                    isCustomer = bool.Parse(tokenS.Claims.First(claim => claim.Type == "isCustomer").Value),
                    Role = int.Parse(tokenS.Claims.First(claim => claim.Type == "Role").Value)
                };
                result = await _repository.AddService(model, auth);
                if (file != null && result.Status != 1)
                {
                    _hostingEnvironment.DeleteImage(model.ImageUrl);
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
        [Route("update")]
        public async Task<ActionResult<DynamicResult>> updateService([FromQuery] string code, [FromForm] ServiceUpdate model, [FromHeader] HeaderParamaters header)
        {
            try
            {
                var result = new DynamicResult();
                var file = model.Image;
                if (file != null)
                {
                    var path = _hostingEnvironment.UploadImage(file, "\\Upload\\Images\\");
                    if (path.Length == 0)
                    {
                        return BadRequest(new DynamicResult()
                        {
                            Message = "File không hợp lệ",
                            Status = 1
                        });
                    }
                    model.ImageUrl = path;
                }
                var handler = new JwtSecurityTokenHandler();
                var tokenStr = header.Authorization.Substring("Bearer ".Length).Trim();
                var jsonToken = handler.ReadToken(tokenStr);
                var tokenS = jsonToken as JwtSecurityToken;

                var auth = new UserLogin()
                {
                    Phone = tokenS.Claims.First(claim => claim.Type == "Phone").Value,
                    Password = tokenS.Claims.First(claim => claim.Type == "Password").Value,
                    isCustomer = bool.Parse(tokenS.Claims.First(claim => claim.Type == "isCustomer").Value),
                    Role = int.Parse(tokenS.Claims.First(claim => claim.Type == "Role").Value)
                };
                result = await _repository.UpdateService(code, model, auth);
                if (file != null && result.Status != 1)
                {
                    _hostingEnvironment.DeleteImage(model.ImageUrl);
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
        [Route("delete")]
        public async Task<ActionResult<DynamicResult>> deleteServiceById([FromQuery] string code, [FromHeader] HeaderParamaters header)
        {
            try
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
                    Role = int.Parse(tokenS.Claims.First(claim => claim.Type == "Role").Value)
                };
                var result = await _repository.DeleteService(code, auth);
                return result;
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