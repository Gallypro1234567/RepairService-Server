
using System.ComponentModel.DataAnnotations;

namespace WorkAppReactAPI.Dtos.Requests
{
    public class UserChangePassword

    {
        
        [Phone]
        public string Phone { get; set; }

        [Required]
        public string OldPassword { set; get; }  
        [Required]
        public string NewPassword { set; get; }  
    }
}