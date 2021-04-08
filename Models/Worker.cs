
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorkAppReactAPI.Models
{
    public class Worker
    {

        [Key]
        public Guid Id { set; get; }
        public string CMND { set; get; } 
        public string ImageUrlOfCMND { set; get; } 
        public virtual User User { set; get; }
        public virtual ICollection<WorkerOfService> WorkerOfCategories { get; set; }

    }
}