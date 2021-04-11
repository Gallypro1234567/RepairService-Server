using System;
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
    public class UserChangePassword 
    {
        
        [Phone]
        public string Phone { get; set; }

        [Required]
        public string OldPassword { set; get; }  
        [Required]
        public string NewPassword { set; get; }  
    }
    public class UserQuery
    {
        public string Code { get; set; }
        public string Phone { get; set; }
        public string Fullname { set; get; }
        public string Address { set; get; }
        public string Birthday { set; get; }
        public bool isCustomer { set; get; }

    }
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
    public class UserUpdate
    {

        
        [Phone]
        public string Phone { set; get; }
        public string Fullname { set; get; }
        public DateTime Birthday { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public string Address { get; set; } 
        public string CMND { get; set; }
        public string ImageUrlOfCMND { get; set; }

    }
}