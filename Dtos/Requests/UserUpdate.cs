
using System;
using System.ComponentModel.DataAnnotations;

namespace WorkAppReactAPI.Dtos.Requests
{
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