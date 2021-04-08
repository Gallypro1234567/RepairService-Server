
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorkAppReactAPI.Models
{
    public class Service
    {

        [Key]
        public Guid Id { set; get; }
        [Required]
        public string Code { set; get; }
        [Required]
        public string Name { set; get; }
        public string Description { set; get; }
        public string ImageUrl { set; get; }
        public DateTime CreateAt { set; get; }
        public virtual ICollection<WorkerOfService> WorkerOfServices { get; set; }
        public virtual ICollection<PreferentialOfService> PreferentialOfServices { get; set; }


    }
}