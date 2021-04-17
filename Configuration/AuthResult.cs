using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WorkAppReactAPI.Configuration
{
    public class AuthResult
    {
        public bool Success { set; get; }
        public List<string> Errors { set; get; }
        public string Token { get; set; }
    }
    public class AuthorResult
    {

        public bool Status { set; get; }
        public string Type { set; get; }
        public string Message { set; get; }
        public List<Dictionary<string, object>> Data { set; get; }
    }

     public class HeaderParamaters
    {

        [FromHeader]
        [Required]
        public string Authorization { get; set; }
    }
 
}