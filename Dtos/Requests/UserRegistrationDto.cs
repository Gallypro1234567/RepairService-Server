using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorkAppReactAPI.Dtos.Requests
{
    public class UserRegistrationDto
    {
        [Required]
        [Phone]
        public string Phone { get; set; }
        [Required]
        [EmailAddress]
        public string Email { set; get; } 

        [Required] 
        public string Password { set; get; }
        
        [Required] 
        public string Fullname { set; get; }

    }
}