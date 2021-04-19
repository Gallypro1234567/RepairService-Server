using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkAppReactAPI.Models
{
    public class User
    {

        [Key]
        public Guid Id { set; get; }

        [Required]
        [MaxLength(250)]
        public string Phone { set; get; }
        [Required]
        [MaxLength(250)]
        public string Password { set; get; }

        [Required]
        [MaxLength(250)]
        public string Fullname { set; get; }
        public DateTime Birthday { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public string Address { get; set; }
        public string RewardPoints { get; set; }
        public int Role { get; set; }
        public bool isOnline { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Worker> Workers { get; set; }
        public virtual ICollection<UserRole> UserRoles { set; get; }
        public int Status { get; set; }  //  0: đã khóa, 1: đang kích hoạt
    }
}