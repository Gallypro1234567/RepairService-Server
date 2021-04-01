using System.Collections.Generic;

namespace WorkAppReactAPI.Configguration
{
    public class AuthResult
    {
        public string Token { get; set; }
        public bool Success { set; get; } 
        public List<string> Errors { set; get; }

    }
}