using System.Data;

namespace WorkAppReactAPI.Configguration
{
    public class SqlResult
    {

        public int TotalRow { set; get; }
        public string Message { set; get; }
        public DataTable dataTable { set; get; }

    }
}