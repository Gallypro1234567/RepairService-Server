using System.Collections.Generic;
using System.Data;
 

namespace WorkAppReactAPI.Controllers
{ 
    public class DynamicResult
    {
       
        public int totalrow { set; get; }
        public string message { set; get; } 
        public List<Dictionary<string, object>> data { set; get; }

    }
    public class SqlResult
    {
         
        public int TotalRow { set; get; }
        public string Message { set; get; }
        public DataTable dataTable { set; get; }

    }

}