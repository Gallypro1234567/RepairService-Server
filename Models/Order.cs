using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkAppReactAPI.Models
{
    public class Order
    {
        [Key] 
        public Guid id { set; get; }
        [Required]

        public string OrderCode { set; get; }
        public HistoryExChange HistoryExChange { set; get; }
        public virtual Customer Customer { get; set; }
    }
}