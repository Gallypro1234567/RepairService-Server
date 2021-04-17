using System.Collections.Generic;

namespace WorkAppReactAPI.Configuration
{
     public class DynamicResult
    {

        
        public int Status { set; get; }
        public string Type { set; get; }
        public string Message { set; get; }
        public List<Dictionary<string, object>> Data { set; get; }
        public int Totalrow { set; get; }

    }
}