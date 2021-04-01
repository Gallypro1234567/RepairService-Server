
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorkAppReactAPI.Models
{
    public class Customer 
    {

        [Key]
        public Guid Id { set; get; }
        public virtual User User {set;get;}
        public virtual ICollection<HistoryAdress> HistotyAddress { set; get; }
        public virtual ICollection<Booking> Bookings { get; set; }

    }
}