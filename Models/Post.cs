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
        public int DistrictId { set; get; }
        public int CityId { set; get; }
        public int WardId { set; get; }
        public string Description { set; get; }
        public string Address { set; get; }

        public string ImageUrl { set; get; }
        [Required]
        public DateTime CreateAt { set; get; }
        public DateTime FinishAt { set; get; }

        [Required]
        public int status { set; get; } // 0: khởi tạo, 1: nhân viên apply, 2 chấp nhận , 3: hoàn thành 
        public int approval { set; get; } // 0: khởi tạo, 1: duyệt thành công , -1: duyệt thất bại   
        public string approvalBy { set; get; }  
        [Required]
        public virtual Customer Customer { get; set; }


        public virtual WorkerOfService WorkerOfService { get; set; }
        public virtual Service Service { get; set; }

    }
}