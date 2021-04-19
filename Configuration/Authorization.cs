using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WorkAppReactAPI.Assets;
using WorkAppReactAPI.Data;
using WorkAppReactAPI.Data.Interface;
using WorkAppReactAPI.Dtos.Requests;

namespace WorkAppReactAPI.Configuration
{
    // class MyAuthorization : ActionFilterAttribute
    // {

    //     private readonly IAuthorRepo _author;
    //     private readonly WorkerServiceContext _context;
        

    //     public override void OnActionExecuting(ActionExecutingContext filterContext)
    //     {
    //         var auth = filterContext.HttpContext.Request.Headers["Authorization"].ToString();
    //         var handler = new JwtSecurityTokenHandler();
    //         var tokenStr = auth.Substring("Bearer ".Length).Trim();
    //         var jsonToken = handler.ReadToken(tokenStr);
    //         var tokenS = jsonToken as JwtSecurityToken;
    //         var user = new UserLogin()
    //         {
    //             Phone = tokenS.Claims.First(claim => claim.Type == "Phone").Value,
    //             Password = tokenS.Claims.First(claim => claim.Type == "Password").Value
    //         };
    //         var usercheck = _context.Users.FirstOrDefault(x =>x.Phone == user.Phone && x.Password == Encryptor.Encrypt(user.Password)); 
    //         if (usercheck == null)
    //         {

    //             filterContext.Result = new BadRequestObjectResult(
    //                  new DynamicResult() { Message = "Authorized is Denied", Data = null, Totalrow = 0, Type = "Error", Status = false }
    //             );

    //         }
    //     }
    // }
}
    
 