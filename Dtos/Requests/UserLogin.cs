using System.ComponentModel.DataAnnotations;

namespace WorkAppReactAPI.Dtos.Requests
{
    public class UserLogin
    {
        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public bool isCustomer { get; set; }
    }
}