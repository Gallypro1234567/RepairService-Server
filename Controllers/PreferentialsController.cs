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

namespace WorkAppReactAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PreferentialsController : ControllerBase
    {

        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IPreferentialRepo _repository;

        public PreferentialsController(IWebHostEnvironment hostEnvironment, IPreferentialRepo repository)
        {
            _hostingEnvironment = hostEnvironment;
            _repository = repository;

        }


        [HttpGet]
        public async Task<ActionResult<DynamicResult>> getAllPreferential([FromQuery] Query model)
        {

            var result = await _repository.ListPreferentials(model);
            return Ok(result);
        }

        [HttpPost]
        [Route("add")]
        public async Task<ActionResult<DynamicResult>> AddPreferential([FromForm] PreferentialUpdate model, [FromHeader] HeaderParamaters header)
        {

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
            var result = await _repository.AddPreferential(model, auth);
            if (file != null && result.Status != 1)
            {
                System.IO.File.Delete(model.ImageUrl);
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("update")]
        public async Task<ActionResult<DynamicResult>> UpdatePreferential([FromQuery]string code,[FromForm] PreferentialUpdate model, [FromHeader] HeaderParamaters header)
        {

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
            var result = await _repository.UpdatePreferential(code , model, auth);
            if (file != null && result.Status != 1)
            {
                System.IO.File.Delete(model.ImageUrl);
            }

            return Ok(result);
        }
        [HttpPost]
        [Route("delete")]
        public async Task<ActionResult<DynamicResult>> DeletePreferential([FromQuery]string code,[FromHeader] HeaderParamaters header)
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
            var result = await _repository.DeletePreferential(code, auth);
            return result;
        }

    }
}