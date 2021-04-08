using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WorkAppReactAPI.Asset;
using WorkAppReactAPI.Configuration;
using WorkAppReactAPI.Data;
using WorkAppReactAPI.Data.Interface;
using WorkAppReactAPI.Dtos.Requests;
using WorkAppReactAPI.Models;
using WorkAppReactAPI.Models.Responses;

namespace WorkAppReactAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly WorkerServiceContext _context;

        private readonly IUserRepo _users;

        private readonly JwtConfig _jwtConfig;
        public AuthController(WorkerServiceContext context, IUserRepo users, IOptionsMonitor<JwtConfig> optionsMonitor, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _users = users;
            _jwtConfig = optionsMonitor.CurrentValue;
            _httpContextAccessor = httpContextAccessor;

        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserRegister model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(c => c.Phone == model.Phone);
                if (existingUser != null)
                {
                    return BadRequest(new RegistrationResponse
                    {
                        Errors = new List<string>()
                            {
                                 "Phone arleay in use"
                            },
                        Success = false
                    });
                }
                var key = Encryptor.Encrypt(model.Password);
               
                var newuser = new UserRegister { Phone = model.Phone, Password = key, Fullname = model.Fullname, isCustomer = model.isCustomer };
                var sqlresult = await _users.Register(newuser);

                if (sqlresult.Status)
                {
                    var jwttoken = GenerateJwtToken(new UserLogin() { Phone = model.Phone, Password = model.Password });
                    return Ok(new RegistrationResponse
                    {
                        Success = true,
                        Token = jwttoken
                    });
                }
                else
                {
                    return BadRequest(new RegistrationResponse
                    {
                        Errors = new List<string>() { "Error" },
                        Success = false
                    });
                }
            }
            return BadRequest(new RegistrationResponse
            {
                Errors = new List<string>(){
                            "Invalid payload"
                        },
                Success = false
            });

        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLogin user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(c => c.Phone == user.Phone);
                if (existingUser == null)
                {
                    return BadRequest(new RegistrationResponse
                    {
                        Errors = new List<string>(){
                            "Invalid Login request"
                        },
                        Success = false
                    });
                }
                var key = Encryptor.Encrypt(user.Password);

                if (existingUser.Password != key)
                {
                    return BadRequest(new RegistrationResponse
                    {
                        Errors = new List<string>(){
                            "Invalid Login request"
                        },
                        Success = false
                    });
                }
                var jwtToken = GenerateJwtToken(new UserLogin() { Phone = existingUser.Phone, Password = user.Password });
                return Ok(new RegistrationResponse
                {
                    Success = true,
                    Token = jwtToken,
                });

            }
            return BadRequest(new RegistrationResponse
            {
                Errors = new List<string>(){
                            "Invalid payload"
                        },
                Success = false
            });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody] UserChangePassword user)
        {
            if (ModelState.IsValid)
            {
                var auth = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
                var handler = new JwtSecurityTokenHandler();
                var tokenStr = auth.Substring("Bearer ".Length).Trim();
                var jsonToken = handler.ReadToken(tokenStr);
                var tokenS = jsonToken as JwtSecurityToken;
               
                var tokenModel = new UserLogin(){
                    Phone = tokenS.Claims.First(claim => claim.Type == "Phone").Value,
                    Password = tokenS.Claims.First(claim => claim.Type == "Password").Value
                };
                var result = await _users.ChangePassword(user, tokenModel);
                return Ok(result);
            }
            return BadRequest(new RegistrationResponse
            {
                Errors = new List<string>(){
                            "Invalid payload"
                        },
                Success = false
            });
        }

        private string GenerateJwtToken(UserLogin user)
        {
            var jwtTokenhandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]{
                      new Claim ("Phone", user.Phone),
                      new Claim ("Password", user.Password),
                      new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                  }),
                Expires = DateTime.UtcNow.AddHours(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = jwtTokenhandler.CreateToken(tokenDescriptor);
            var jwttoken = jwtTokenhandler.WriteToken(token);
            return jwttoken;
        }
    }
}