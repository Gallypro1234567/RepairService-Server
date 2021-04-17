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
        public string Positon { set; get; }

        [Required]
        public DateTime CreateAt { set; get; }
        public DateTime FinishAt { set; get; }
        public string Note { set; get; }

        [Required]
        public int status { set; get; }

        [Required]
        public virtual Customer Customer { get; set; }
    
        [Required]
        public virtual WorkerOfService WorkerOfService { get; set; }

    }
}