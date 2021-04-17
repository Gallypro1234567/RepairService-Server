using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using WorkAppReactAPI.Assets;
using WorkAppReactAPI.Configuration;
using WorkAppReactAPI.Data.Interface;
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
        public async Task<ActionResult<DynamicResult>> getAllPreferential()
        {

            var result = await _repository.ListPreferentials();
            return result;
        }

        [HttpPost]
        [Route("add")]
        public async Task<ActionResult<DynamicResult>> AddPreferential([FromForm] PreferentialUpdate model)
        {

            var file = model.Image;
            if (file.Length > 0)
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
            var result = await _repository.AddPreferential(model);
            if (result.Status != 1)
            {
                System.IO.File.Delete(model.ImageUrl);
            }
            return result;
        }

        [HttpPost]
        [Route("update")]
        public async Task<ActionResult<DynamicResult>> UpdatePreferential([FromBody] PreferentialUpdate model)
        {

            var file = model.Image;
            if (file.Length > 0)
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
            var result = await _repository.UpdatePreferential(model);
            if (result.Status != 1)
            {
                System.IO.File.Delete(model.ImageUrl);
            }

            return result;
        }

    }
}