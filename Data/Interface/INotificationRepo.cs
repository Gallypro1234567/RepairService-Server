using System;
using System.Threading.Tasks;
using WorkAppReactAPI.Configuration;
using WorkAppReactAPI.Dtos;
using WorkAppReactAPI.Dtos.Requests;

namespace WorkAppReactAPI.Data.Interface
{
    public interface INotificationRepo
    {
        Task<DynamicResult> Notifications(NotifiQuery model);
        Task<DynamicResult> AddNotifications(NotificationUpdate model, UserLogin auth);
        Task<DynamicResult> UpdateStatusNotifications(string code, UserLogin auth);
        Task<DynamicResult> DeleteNotifications(string code, UserLogin auth);

    }
    public class NotifiQuery
    {
        public int start { set; get; }
        public int length { set; get; }
        public int status { set; get; }
        public int type { set; get; }
        public string code { set; get; }
        public string phone { set; get; }

    }
    public class NotificationUpdate
    {
        public String Code { set; get; }
        public String Title { set; get; }
        public String Content { set; get; }
        public DateTime CreateAt { set; get; }
        public String SendBy { set; get; }
        public String ReceiveBy { set; get; }
        public String PostCode { set; get; }
        public int SendByDelete { set; get; } // xóa bởi người tạo
        public int ReceiveByDelete { set; get; } // xóa bởi người nhận
        public int isReaded { set; get; }
        public int type { set; get; }
        public int status { set; get; }

    }
}