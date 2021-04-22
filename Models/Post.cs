using System;
using System.ComponentModel.DataAnnotations;


namespace WorkAppReactAPI.Models
{
    public class Post
    {
        [Key]
        public Guid Id { set; get; } 
        [Required]
        public string Code { set; get; }
        public string Title { set; get; }
        public string Position { set; get; }  
        public string Address { set; get; } 
        public string ImageUrl { set; get; } 
        [Required]
        public DateTime CreateAt { set; get; }
        public DateTime FinishAt { set; get; } 

        [Required]
        public int status { set; get; } // 0: khởi tạo, 1: worker booked, 2 đang thực hiện, 3: hoàn thành

        [Required]
        public virtual Customer Customer { get; set; }

   
        public virtual WorkerOfService WorkerOfService { get; set; }
        public virtual Service Service { get; set; }

    }
}