using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkAppReactAPI.Models
{
    public class UserRole
    {

        [Key]
        public Guid Id { set; get; } 
        public virtual User User { set; get; }
        public virtual Role Role { set; get; }
        public DateTime CreateAt { set; get; } 
    }
}