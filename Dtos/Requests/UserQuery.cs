
using System.ComponentModel.DataAnnotations;

namespace WorkAppReactAPI.Dtos.Requests
{
    public class UserQuery
    {
        public string Code { get; set; }
        public string Phone { get; set; }
        public string Fullname { set; get; }
        public string Address { set; get; }
        public string Birthday { set; get; }
        public bool isCustomer { set; get; }

    }
}