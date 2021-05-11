
using System;
using System.ComponentModel.DataAnnotations;

namespace WorkAppReactAPI.Models
{
    public class Feedback
    {

        [Key]
        public Guid Id { set; get; }
        public string Code { set; get; }
        public string WorkerOfServiceCode { set; get; }
        public string PostCode { set; get; }
        public string Description { set; get; }
        public int PointRating { set; get; }
        public DateTime? CreateAt { set; get; }
    }
}