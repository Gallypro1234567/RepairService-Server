using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkAppReactAPI.Models
{
    public class Customer
    {

        [Key] 
        public Guid id { get; set; }

        [Required]
        [MaxLength(500)]
        public string CompanyName { get; set; }

        public User User { get; set; }


        public virtual ICollection<Order> Orders { get; set; }

    }
}