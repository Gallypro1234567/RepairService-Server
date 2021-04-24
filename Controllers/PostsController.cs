using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using WorkAppReactAPI.Assets;
using WorkAppReactAPI.Configuration;
using WorkAppReactAPI.Data;
using WorkAppReactAPI.Data.Interface;
using WorkAppReactAPI.Dtos.Requests;
using WorkAppReactAPI.Models;

namespace WorkAppReactAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostsController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        private readonly IPostRepo _repository;
        public PostsController(IWebHostEnvironment hostEnvironment, IPostRepo repository)
        {
            _hostingEnvironment = hostEnvironment;
            _repository = repository;

        }


        [Authorize]
        [HttpGet]
        public async Task<ActionResult<DynamicResult>> getPosts([FromQuery] PostGet model)
        {

            var result = await _repository.GetPost(model);
            return Ok(result);
        }
        [Authorize]
        [HttpGet]
        [Route("Recently")]
        public async Task<ActionResult<DynamicResult>> getRecentlyPosts([FromQuery] PostGet model)
        {

            var result = await _repository.GetRecentlyPosts(model);
            return Ok(result);
        }
        [Authorize]
        [HttpGet("{phone}")]
        public async Task<ActionResult<DynamicResult>> getPostsByUser(string phone, [FromQuery] PostGet model)
        {

            var result = await _repository.GetPostByPhone(phone, model);
            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        [Route("add")]
        public async Task<ActionResult<DynamicResult>> addPost([FromForm] PostUpdate model, [FromHeader] HeaderParamaters header)
        {
            var file = model.Image;
            var result = new DynamicResult();
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
                Role = int.Parse(tokenS.Claims.First(claim => claim.Type == "Role").Value),
            };
            result = await _repository.InserPost(model, auth);
            if (file != null && result.Status != 1)
            {
                System.IO.File.Delete(model.ImageUrl);
            }
            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        [Route("updatebycustomer")]
        public async Task<ActionResult<DynamicResult>> updatePostbyCustomer([FromQuery] string code, [FromForm] PostUpdate model, [FromHeader] HeaderParamaters header)
        {
            var file = model.Image;
            var result = new DynamicResult();
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
                Role = int.Parse(tokenS.Claims.First(claim => claim.Type == "Role").Value),
            };
            result = await _repository.UpdatePostByCustomer(code, model, auth);
            if (file != null && result.Status != 1)
            {
                System.IO.File.Delete(model.ImageUrl);
            }
            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        [Route("updatebyworker")]
        public async Task<ActionResult<DynamicResult>> updatePostbyWorker([FromQuery] string code, [FromHeader] HeaderParamaters header)
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
            result = await _repository.UpdatePostByWorker(code, auth);
            return Ok(result);
        }
        [Authorize]
        [HttpPost]
        [Route("delete")]
        public async Task<ActionResult<DynamicResult>> deletePostById([FromQuery] string code, [FromHeader] HeaderParamaters header)
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
            result = await _repository.DeletePost(code, auth);
            return Ok(result);
        }
    }
}