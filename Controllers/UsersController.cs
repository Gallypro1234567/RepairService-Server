
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkAppReactAPI.Configuration;
using WorkAppReactAPI.Data.Interface;
using WorkAppReactAPI.Dtos;
using WorkAppReactAPI.Dtos.Requests;
using WorkAppReactAPI.Models.Responses;

namespace WorkAppReactAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : ControllerBase
    {
        private readonly ICustomerRepo _CustomerRepository;
        private readonly IWorkerRepo _workerRepository;

        public UsersController(ICustomerRepo CustomerRepository, IWorkerRepo workerRepository)
        {
            _CustomerRepository = CustomerRepository;
            _workerRepository = workerRepository;
        }
        
        //GET Customer 
        [Authorize]
        [HttpGet]
        [Route("getinfo")]
        public async Task<ActionResult<DynamicResult>> getinfo([FromQuery] string phone)
        {
            var result = await _CustomerRepository.getinfo(phone);
            return Ok(result);
        } 
         //GET Customer
        [HttpGet]
        [Route("customers")]
        public async Task<ActionResult<DynamicResult>> getCustomer([FromQuery] Query model)
        {
            var result = await _CustomerRepository.getCustomer(model);
            return Ok(result);
        }
        [HttpGet]
        [Route("customers/{phone}/detail")]
        public async Task<ActionResult<DynamicResult>> getCustomerbyPhone(string phone)
        {
            var result = await _CustomerRepository.GetCustomerByPhone(phone);
            return Ok(result);
        }

        [HttpGet]
        [Route("customers/{phone}/delete")]
        public async Task<ActionResult<DynamicResult>> DeleteCustomerByphone(string phone, [FromHeader] HeaderParamaters header)
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
                    Role = int.Parse(tokenS.Claims.First(claim => claim.Type == "Role").Value),
                };
                var result = await _CustomerRepository.DeleteCustomer(phone, auth);
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


        //GET worker
        [HttpGet]
        [Route("workers")]
        public async Task<ActionResult<DynamicResult>> getWorker([FromQuery] Query model)
        {
            var result = await _workerRepository.getWorker(model);
            return Ok(result);
        }
        [HttpGet]
        [Route("workers/{phone}/detail")]
        public async Task<ActionResult<DynamicResult>> getWorkerbyPhone(string phone)
        {
            var result = await _workerRepository.GetWorkerByPhone(phone);
            return Ok(result);
        }

        [HttpGet]
        [Route("workers/{phone}/delete")]
        public async Task<ActionResult<DynamicResult>> DeleteWorkerByphone(string phone, [FromHeader] HeaderParamaters header)
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
                    Role = int.Parse(tokenS.Claims.First(claim => claim.Type == "Role").Value),
                };
                var result = await _workerRepository.DeleteWorker(phone, auth);
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