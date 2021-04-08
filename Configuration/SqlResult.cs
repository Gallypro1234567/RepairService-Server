using System.Data;

namespace WorkAppReactAPI.Configuration
{
    public class SqlResult
    {

        public bool Status { set; get; }
        public string Type { set; get; }
        public int TotalRow { set; get; }
        public string Message { set; get; }
        public DataTable dataTable { set; get; }

    }
}