
using System;
using System.ComponentModel.DataAnnotations;

namespace WorkAppReactAPI.Models
{
    public class ApplyToPost
    {

        [Key]
        public Guid Id { set; get; }
        public String WorkerOfServiceCode { get; set; }
        public String PostCode { get; set; }
        public int Status { set; get; }
        public DateTime CreateAt { set; get; }
        public DateTime? AcceptAt { set; get; }

    }
}