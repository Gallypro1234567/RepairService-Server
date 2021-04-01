
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorkAppReactAPI.Models
{
    public class WorkerOfService
    {

        [Key]
        public Guid Id { set; get; }
        public string Position { set; get; }
        public bool isOnline { set; get; }
        public bool isApproval { set; get; }
        public DateTime CreateAt { set; get; }

        public virtual Service Service { get; set; }
        public virtual Worker Worker { get; set; }
        public virtual Feelback Feelback { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }


    }
}