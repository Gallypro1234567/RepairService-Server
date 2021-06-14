using System;
using System.ComponentModel.DataAnnotations;


namespace WorkAppReactAPI.Models
{
    public class Notification
    {
        [Key]
        public Guid Id { set; get; }
        public String Code { set; get; }
        public String Title { set; get; }
        public String Content { set; get; }
        public DateTime CreateAt { set; get; }
        public String SendBy { set; get; }
        public String ReceiveBy { set; get; }
        public String PostCode { set; get; }
        public int SendByDelete { set; get; } // xóa bởi người tạo
        public int ReceiveByDelete { set; get; } // xóa bởi người nhận
        public int status { set; get; } // 0: không chấp nhận,  1: chấp nhận , 2: hoàn tất, 3: đã đánh giá
        public int isReaded { set; get; } // 0: chưa xem, 1: chưa xem
        public int type { set; get; } // 0: tin hệ thống, 1: tin giao dịch,  2: tin ứng tuyển
    }
}