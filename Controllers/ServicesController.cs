using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using WorkAppReactAPI.Assets;
using WorkAppReactAPI.Configuration;
using WorkAppReactAPI.Data.Interface;
using WorkAppReactAPI.Dtos.Requests;
using WorkAppReactAPI.Models;

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
        public async Task<ActionResult<DynamicResult>> getAllService()
        {

            var list = await _repository.getListService();
            return list;
        }

        [Authorize]
        [HttpPost]
        [Route("add")]
        public async Task<ActionResult<DynamicResult>> addService([FromForm] ServiceUpdate model)
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
            var result = await _repository.AddService(model);
            if (result.Status != 1)
            {
                System.IO.File.Delete(model.ImageUrl);
            }
            return result;
        }
        [Authorize]
        [HttpPost]
        [Route("update")]
        public async Task<ActionResult<DynamicResult>> updateService([FromForm] ServiceUpdate model)
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
            var result = await _repository.UpdateService(model);
            if (result.Status != 1)
            {
                System.IO.File.Delete(model.ImageUrl);
            }
            return result;
        }
        [Authorize]
        [HttpPost]
        [Route("delete")]
        public async Task<ActionResult<DynamicResult>> deleteServiceById([FromBody] ServiceDrop model)
        {

            var list = await _repository.DeleteService(model);
            return list;
        }
    }
}