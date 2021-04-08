using System.ComponentModel.DataAnnotations;

namespace WorkAppReactAPI.Dtos.Requests
{
    public class ServicePost
    {
        [Required]
        public string Code { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; } 
        public string Description { get; set; }
    }
}