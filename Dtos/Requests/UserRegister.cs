
using System.ComponentModel.DataAnnotations;

namespace WorkAppReactAPI.Dtos.Requests
{
    public class UserRegister
    {
     
        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        public string Password { set; get; }

        [Required]
        public string Fullname { set; get; }

        [Required]
        public bool isCustomer { set; get; }

    }
}