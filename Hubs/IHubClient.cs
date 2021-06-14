using System.Threading.Tasks;

namespace WorkAppReactAPI.Data.Interface
{

    public interface IHubClient
    {
        Task BroadcastMessage(MessageInstance msg);
    }
    public class MessageInstance
    {
        public string Timestamp { get; set; }
        public string From { get; set; }
        public string Message { get; set; }
    }
}