using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;


namespace WorkAppReactAPI.Data
{
    public class SignalrHub : Hub 
    {
        public async Task MoveViewFromServer(float newX, float newY)
        {
            await Clients.Others.SendAsync("ReceiveNewPosition", newX, newY);
        }

    }
}