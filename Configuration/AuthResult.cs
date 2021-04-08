using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace WorkAppReactAPI.Configuration
{
    public class AuthResult
    {
        public bool Success { set; get; }
        public List<string> Errors { set; get; }  
        public string Token { get; set; } 
    }
}