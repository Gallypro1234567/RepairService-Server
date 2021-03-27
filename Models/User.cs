using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkAppReactAPI.Models
{
    public class User
    {

        [Key] 
        public Guid id { set; get; }

        [Required]
        [MaxLength(250)]
        public string name { set; get; }

        [Required]
        [MaxLength(250)]
        public string address { set; get; }
        
        [Required]
        public DateTime Birthday { get; set; }

    }
}