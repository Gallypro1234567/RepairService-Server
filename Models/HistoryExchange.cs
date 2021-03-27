using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkAppReactAPI.Models
{
    public class HistoryExChange
    {
         
        [Key]
        public Guid id { set; get; }

        [Required]  
        public decimal Price { set; get; }

        [Required] 
        public string Content { set; get; }
    }
}