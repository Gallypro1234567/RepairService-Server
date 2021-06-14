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
    public class WorkerOfServicesController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        private readonly IWorkerOfServicesRepo _workerrepository;

        public WorkerOfServicesController(IWebHostEnvironment hostEnvironment, IWorkerOfServicesRepo workerrepository)
        {
            _hostingEnvironment = hostEnvironment;
            _workerrepository = workerrepository;

        }


        [Authorize]
        [HttpGet]
        public async Task<ActionResult<DynamicResult>> getWorkerOfServices([FromQuery] Query model)
        {
            var result = await _workerrepository.GetallWorkerOfServices(model);
            return Ok(result);
        }
        [Authorize]
        [HttpGet("{phone}")]
        public async Task<ActionResult<DynamicResult>> getWorkerOfServicesDetail(string phone, [FromQuery] Query model)
        {
            var result = await _workerrepository.GetWorkerOfServicesByUser(phone, model);
            return Ok(result);
        }
        [Authorize]
        [HttpGet("{phone}/detail")]
        public async Task<ActionResult<DynamicResult>> getWorkerOfServicesDetail(string phone, [FromQuery] String code)
        {
            var result = await _workerrepository.GetWorkerOfServicesDetailByCode(phone, code);
            return Ok(result);
        }
        [Authorize]
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<DynamicResult>> Register([FromForm] WorkerOfServicesUpdate model, [FromHeader] HeaderParamaters header)
        {
            try
            {
                var formFiles = model.Images;
                var result = new DynamicResult();
                var list = new List<String>();
                if (model.Images != null)
                    foreach (var file in formFiles)
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
                        list.Add(path);

                    }

                 model.ImageUrl = list.Count > 0 ? String.Join(",", list.ToArray()) :   "";

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
                result = await _workerrepository.RegisterWorkerOfServices(model, auth);
                if (formFiles != null && result.Status != 1)
                {
                    foreach (var imageUrl in list)
                    {
                        _hostingEnvironment.DeleteImage(imageUrl);
                    }

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
        [Route("Vetification")]
        public async Task<ActionResult<DynamicResult>> Vetification([FromForm] WorkerOfServicesUpdate model, [FromHeader] HeaderParamaters header)
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
                result = await _workerrepository.VetificationWorkerOfServices(model, auth);

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
        public async Task<ActionResult<DynamicResult>> deletePostById([FromQuery] string code, [FromHeader] HeaderParamaters header)
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
                result = await _workerrepository.DeleteWorkerOfServices(code, auth);
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