using System;
using System.ComponentModel.DataAnnotations;

namespace WorkAppReactAPI.Models
{
    public class HistoryAdress
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Positon { get; set; }
        [Required]
        public string AddressText { set; get; }
        public virtual Customer Customer { set; get; }
    }
}