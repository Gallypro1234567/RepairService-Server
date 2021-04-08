using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class Authorization : AuthorizeAttribute
{
    private readonly string allowedFuntions;
    private readonly int typeFuntion;
    private readonly bool ajax;
    public Authorization(string funtions, int typeFuntions, bool isAjax)
    {

            this.allowedFuntions = funtions;
            this.ajax = isAjax;
            this.typeFuntion = typeFuntions;
    }
    // public  override void OnAuthorization(AuthorizationFilterContext context)
    // {
    //     var user = context.HttpContext.Items["User"];
    //     if (user == null)
    //     {
    //         // not logged in
    //         context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
    //     }
    // }
}