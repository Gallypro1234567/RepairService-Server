
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorkAppReactAPI.Models
{
    public class Service
    {

        [Key]
        public Guid Id { set; get; }
        public string Name { set; get; }
        public string Code { set; get; }
        public string Note { set; get; }

        public DateTime CreateAt { set; get; }
        public virtual ICollection<WorkerOfService> WorkerOfServices { get; set; }
        public virtual ICollection<PreferentialOfService> PreferentialOfServices { get; set; }
       

    }
}