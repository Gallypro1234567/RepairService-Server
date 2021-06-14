using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System; 
using System.Linq;
using System.Threading.Tasks; 
using WorkAppReactAPI.Data;
using WorkAppReactAPI.Data.Interface;

namespace WorkAppReactAPI.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly IUserConnectionManager _userConnectionManager;

        private readonly WorkerServiceContext _context;

        public NotificationHub(IUserConnectionManager userConnectionManager, WorkerServiceContext context)
        {
            _userConnectionManager = userConnectionManager;
            _context = context;
        }
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            var httpContext = this.Context.GetHttpContext();
            var userId = httpContext.Request.Query["userId"];
            _userConnectionManager.KeepUserConnection(userId, Context.ConnectionId);
            var notifi = await _context.Notifications.Where(x => x.SendBy == userId && x.isReaded == 0).ToListAsync();
            foreach (var item in notifi)
            {
                await Clients.Caller.SendAsync("sendToUser", item.Title, item.Content, item.SendBy, item.CreateAt, item.isReaded);
            }

        }
         
        public async override Task OnDisconnectedAsync(Exception exception)
        {
            //get the connectionId
            var connectionId = Context.ConnectionId;
            _userConnectionManager.RemoveUserConnection(connectionId);
            var value = await Task.FromResult(0);//adding dump code to follow the template of Hub > OnDisconnectedAsync
        }

    }
}
