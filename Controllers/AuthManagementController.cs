using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WorkAppReactAPI.Data;
using WorkAppReactAPI.Dtos.Requests;
using WorkAppReactAPI.Models.Responses;

namespace WorkAppReactAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthManagementController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _Usermanager;
        private readonly JwtConfig _jwtConfig;
        public AuthManagementController(UserManager<IdentityUser> userManager, IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _Usermanager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _Usermanager.FindByEmailAsync(user.Email);
                if (existingUser != null)
                {
                    return BadRequest(new RegistrationResponse
                    {
                        Errors = new List<string>(){
                                                "Email arleay in use"
                                            },
                        Success = false
                    });
                }
                var newuser = new IdentityUser { Email = user.Email, UserName = user.Email };
                var isCreated = await _Usermanager.CreateAsync(newuser, user.Password);
                if (isCreated.Succeeded)
                {
                    var jwttoken = GenerateJwtToken(newuser);
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
                        Errors = isCreated.Errors.Select(x => x.Description).ToList(),
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
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _Usermanager.FindByEmailAsync(user.Email);
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
                var isCorrect = await _Usermanager.CheckPasswordAsync(existingUser, user.Password);
                if (!isCorrect)
                {
                    return BadRequest(new RegistrationResponse
                    {
                        Errors = new List<string>(){
                            "Invalid Login request"
                        },
                        Success = false
                    });
                }
                var jwtToken = GenerateJwtToken(existingUser);
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
        private string GenerateJwtToken(IdentityUser user)
        {
            var jwtTokenhandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]{
                      new Claim ("Id", user.Id),
                      new Claim (JwtRegisteredClaimNames.Email, user.Email),
                      new Claim (JwtRegisteredClaimNames.Sub, user.Email),
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