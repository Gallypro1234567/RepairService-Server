using System.ComponentModel.DataAnnotations;

namespace WorkAppReactAPI.Dtos.Requests
{
    public class UserLoginRequest
    {
        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        public string Password { get; set; }
    }
}