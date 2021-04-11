
using System;
using System.ComponentModel.DataAnnotations;

namespace WorkAppReactAPI.Models
{
    public class PreferentialOfService
    {

        [Key]
        public Guid Id { get; set; } 

        [Required]
        public virtual Service Service { set; get; }
        [Required]
        public virtual Preferential Preferential { set; get; }

    }
}