using System.Collections.Generic;

namespace WorkAppReactAPI.Configguration
{
     public class DynamicResult
    {

        public int totalrow { set; get; }
        public string message { set; get; }
        public List<Dictionary<string, object>> data { set; get; }

    }
}