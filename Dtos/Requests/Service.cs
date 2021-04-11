using System;
using System.ComponentModel.DataAnnotations;

namespace WorkAppReactAPI.Dtos.Requests
{
    public class ServiceUpdate
    {
        
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public string ImageUrl { get; set; } 
        public string Description { get; set; }
        public DateTime CreateAt { get; set; }
    }
    
    public class ServiceDrop
    {
        [Required]
        public string Code { get; set; } 
    }
}