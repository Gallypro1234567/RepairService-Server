
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorkAppReactAPI.Models
{
    public class WorkerOfService
    {

        [Key]
        public Guid Id { set; get; }
        public string Code { set; get; }
        public string Position { set; get; }
        public bool isOnline { set; get; } 
        public int isApproval { set; get; } // 0 : chưa duyệt, 1: duyệt thành công, 2 : duyệt thất bại
        public DateTime CreateAt { set; get; } 
        public virtual Service Service { get; set; }
        public virtual Worker Worker { get; set; }
        public virtual ICollection<Post> Bookings { get; set; }


    }
}