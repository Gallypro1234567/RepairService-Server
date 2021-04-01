using WorkAppReactAPI.Assets;
using WorkAppReactAPI.Controllers;
using WorkAppReactAPI.Data.Interface;

namespace WorkAppReactAPI.Data
{
    public class SqlServiceRepo : IServiceRepo
    {
        
        private readonly WorkerServiceContext _context;
        public SqlServiceRepo(WorkerServiceContext context)
        {
            _context = context;
        }

        public DynamicResult getListService()
        {
            var result = _context.ExecuteDataTable("[dbo].[sp_GetServices]", null).JsonData(); 
            return result;
        }
    }
}