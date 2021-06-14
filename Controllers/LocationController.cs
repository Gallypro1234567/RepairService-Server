using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using WorkAppReactAPI.Assets;
using WorkAppReactAPI.Configuration;
using WorkAppReactAPI.Data.Interface;
using WorkAppReactAPI.Dtos.Requests;
using WorkAppReactAPI.Models.Responses;

namespace WorkAppReactAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LocationController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public LocationController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public class Province
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
        [HttpGet("provinces")]
        public ActionResult<DynamicResult> getprovince([FromHeader] HeaderParamaters header)
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
                XElement xElement = XElement.Load(@"" + _webHostEnvironment.WebRootPath + "/location.xml");
                var citys = (from s in xElement.Descendants("row")
                             select new Dictionary<string, object>()
                             {
                                 ["Id"] = s.Element("Id").Value,
                                 ["Name"] = s.Element("Name").Value,
                             }).ToList();
                return Ok(
                    new DynamicResult()
                    {
                        Status = 1,
                        Message = "Success",
                        Data = citys,

                    }
                );
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new DynamicResult()
                    {
                        Status = 1,
                        Message = ex.Message,

                    }
                );
            }
        }
        [HttpGet("districts")]
        public ActionResult<DynamicResult> getdistrict([FromQuery] int cityId, [FromHeader] HeaderParamaters header)
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
                XElement xElement = XElement.Load(@"" + _webHostEnvironment.WebRootPath + "/location.xml");
                var districts = (from s in xElement.Descendants("row").Where(element => int.Parse(element.Element("Id").Value) == cityId).Descendants("Districts")
                                 select new Dictionary<string, object>()
                                 {
                                     ["Id"] = s.Element("Id").Value,
                                     ["Name"] = s.Element("Name").Value,
                                 }).ToList();
                return Ok(
                    new DynamicResult()
                    {
                        Status = 1,
                        Message = "Success",
                        Data = districts,

                    }
                );
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new DynamicResult()
                    {
                        Status = 1,
                        Message = ex.Message,

                    }
                );
            }

        }
        [HttpGet("wards")]
        public ActionResult<DynamicResult> getward([FromQuery] int cityId, [FromQuery] int districtid, [FromHeader] HeaderParamaters header)
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
                XElement xElement = XElement.Load(@"" + _webHostEnvironment.WebRootPath + "/location.xml");
                var wards = (from s in xElement.Descendants("row")
                                .Where(element => int.Parse(element.Element("Id").Value) == cityId)
                                .Descendants("Districts").Where(element => int.Parse(element.Element("Id").Value) == districtid).Descendants("Wards")
                             select new Dictionary<string, object>()
                             {
                                 ["Id"] = s.Element("Id").Value,
                                 ["Name"] = s.Element("Name").Value,
                             }).ToList();
                return Ok(
                    new DynamicResult()
                    {
                        Status = 1,
                        Message = "Success",
                        Data = wards,

                    }
                );
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new DynamicResult()
                    {
                        Status = 1,
                        Message = ex.Message,

                    }
                );
            }

        }
    }
}