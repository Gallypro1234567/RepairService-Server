using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace WorkAppReactAPI.Configguration
{
    public class AuthResult
    {
        public string Token { get; set; }
        [FromHeader]
        public string Authorization { get; set; }
        public bool Success { set; get; } 
        public List<string> Errors { set; get; }

    }
}