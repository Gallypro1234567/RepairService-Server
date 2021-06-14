using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WorkAppReactAPI.Configuration;
using WorkAppReactAPI.Data.Interface;
using WorkAppReactAPI.Dtos.Requests;
using WorkAppReactAPI.Hubs;

namespace WorkAppReactAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NotificationsController : ControllerBase
    {
        public class Article
        {
            public string articleHeading { get; set; }
            public string articleContent { get; set; }
            public string userId { get; set; }
        }
        private readonly IHubContext<NotificationHub> _notificationHubContext;

        private readonly INotificationRepo _repository;
        private readonly IUserConnectionManager _userConnectionManager;

        public NotificationsController(IHubContext<NotificationHub> notificationHubContext, INotificationRepo repository, IUserConnectionManager userConnectionManager)
        {

            _notificationHubContext = notificationHubContext;
            _userConnectionManager = userConnectionManager;
            _repository = repository;
        }


        [HttpPost]
        public async Task<ActionResult<DynamicResult>> SendToAllUser([FromQuery] NotificationUpdate model, [FromHeader] HeaderParamaters header)
        {
            try
            {
                await _notificationHubContext.Clients.All.SendAsync("sendToUser", model.Title, model.Content);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new DynamicResult()
                    {
                        Message = ex.Message,
                        Status = 2,

                    }
                );
            }

        }
        [HttpGet]
        public async Task<ActionResult<DynamicResult>> get([FromQuery] NotifiQuery query)
        {

            var result = await _repository.Notifications(query);
            return Ok(result);
        }

        [HttpPost]
        [Route("SendToSpecificUser")]
        public async Task<ActionResult<DynamicResult>> SendToSpecificUser([FromForm] NotificationUpdate model, [FromHeader] HeaderParamaters header)
        {
            try
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
                result = await _repository.AddNotifications(model, auth);
                if (result.Status == 1)
                {
                    var data = result.Data[0];
                    var a = data["Code"].ToString();
                    var connections = _userConnectionManager.GetUserConnections(model.ReceiveBy);
                    if (connections != null && connections.Count > 0)
                    {
                        foreach (var connectionId in connections)
                        {
                            await _notificationHubContext.Clients.Client(connectionId)
                                .SendAsync(
                                "sendToUser",
                                model.Code,
                                model.Title, 
                                model.Content,  
                                model.SendBy,
                                model.ReceiveBy,
                                model.CreateAt,
                                model.isReaded
                            );
                        }
                    }
                    return Ok(result);
                }
                return BadRequest(result);

            }
            catch (Exception ex)
            {
                return BadRequest(
                    new DynamicResult()
                    {
                        Message = ex.Message,
                        Status = 2,

                    }
                );
            }

        }
        [HttpPost]
        [Route("update")]
        public async Task<ActionResult> isReaded([FromQuery] string code, [FromHeader] HeaderParamaters header)
        {
            try
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
                // var connections = _userConnectionManager.GetUserConnections(model.phone);
                // if (connections != null && connections.Count > 0)
                // {
                //     await _repository.UpdateStatusNotifications(model.code, auth);
                // }
                result = await _repository.UpdateStatusNotifications(code, auth);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new DynamicResult()
                    {
                        Message = ex.Message,
                        Status = 2,

                    }
                );
            }

        }
        [HttpPost]
        [Route("delete")]
        public async Task<ActionResult> DeleteNotification([FromQuery] string code, [FromHeader] HeaderParamaters header)
        {
            try
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
                // var connections = _userConnectionManager.GetUserConnections(model.phone);
                // if (connections != null && connections.Count > 0)
                // {
                //     await _repository.DeleteNotifications(model.code, auth);
                // }
                result = await _repository.DeleteNotifications(code, auth);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new DynamicResult()
                    {
                        Message = ex.Message,
                        Status = 2,

                    }
                );
            }

        }
    }
}