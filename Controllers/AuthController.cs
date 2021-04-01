using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WorkAppReactAPI.Asset;
using WorkAppReactAPI.Configguration;
using WorkAppReactAPI.Data;
using WorkAppReactAPI.Dtos.Requests;
using WorkAppReactAPI.Models;
using WorkAppReactAPI.Models.Responses;

namespace WorkAppReactAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly WorkerServiceContext _context;

        private readonly JwtConfig _jwtConfig;
        public AuthController(WorkerServiceContext context, IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _context = context;
            _jwtConfig = optionsMonitor.CurrentValue;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(c => c.Phone == user.Phone);
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
                var key = Encryptor.Encrypt(user.Password);

                var newuser = new User { Id = Guid.NewGuid(), Phone = user.Phone, Password = key, Fullname = user.Fullname, Email = user.Email };
                await _context.Users.AddAsync(newuser);
                var isCreated = await _context.SaveChangesAsync();

                if (isCreated > 0)
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
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user,[FromHeader] AuthResult headers)
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
                var isCorrect = await _context.Users.FirstOrDefaultAsync(c => c.Password == key);

                if (isCorrect == null)
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
        private string GenerateJwtToken(User user)
        {
            var jwtTokenhandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]{
                      new Claim ("Id", user.Id.ToString()),
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