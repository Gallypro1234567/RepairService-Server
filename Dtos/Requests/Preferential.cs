using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorkAppReactAPI.Dtos.Requests
{
    public class PreferentialUpdate
    {

        public Guid Id { set; get; }
        
        public string Code { set; get; }
        [Required]
        public string Title { set; get; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public double Percents { set; get; }
        [Required]
        public DateTime FromDate { set; get; }
        [Required]
        public DateTime ToDate { set; get; } 
        public string ListService { get; set; }
    }

    public class PreferentialDrop
    {

        [Required]
        public string Code { get; set; }

    }
}