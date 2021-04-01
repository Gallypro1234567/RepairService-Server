using Microsoft.AspNetCore.Mvc;


namespace WorkAppReactAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public ActionResult<DynamicResult> Login()
        {
            return Ok();
        }
        
        public ActionResult<DynamicResult> Register()
        {
            return Ok();
        }
    }
}