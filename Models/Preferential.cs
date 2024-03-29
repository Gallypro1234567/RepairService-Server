
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorkAppReactAPI.Models
{
    public class Preferential
    {

        [Key]
        public Guid Id { set; get; }
        [Required]
        public string Code { set; get; }
        [Required]
        public string Title { set; get; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        [Required]
        public double Percents { set; get; }
        [Required]
        public DateTime FromDate { set; get; }
        [Required]
        public DateTime ToDate { set; get; }
        public virtual ICollection<PreferentialOfService> PreferentialOfServices { get; set; }

    }
}