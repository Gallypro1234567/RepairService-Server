
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorkAppReactAPI.Models
{
    public class Feelback
    {

        [Key]
        public Guid Id { set; get; }

        [Required] 
        public string Code { set; get; }

        public virtual ICollection<WorkerOfService> WorkerOfServices {set;get;}

    }
}