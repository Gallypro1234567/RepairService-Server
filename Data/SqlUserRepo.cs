using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WorkAppReactAPI.Assets;
using WorkAppReactAPI.Models;

namespace WorkAppReactAPI.Data
{
    public class SqlUserRepo : IUserRepo
    {

        private readonly WorkerServiceContext _context;
        public SqlUserRepo(WorkerServiceContext context)
        {
            _context = context;
        }
        public IEnumerable<User> getAllUser()
        {
            return _context.Users.ToList();
        }

        public User GetUserById(Guid id)
        {
            var connnectstring = _context.Database.GetConnectionString();
            // var abc = _context.Database.ExecuteSqlRaw("dbo.sp_GetCusctomer");
            var a = _context.Database.ExecuteSqlRaw("EXEC [dbo].[sp_GetCustomer]");

            return _context.Users.FirstOrDefault(x => x.id == id);
        }

        public System.Data.DataTable GetUserByIds(Guid id)
        {
            var datatable = _context.ExecuteDataTable("EXEC [dbo].[sp_GetCustomer]", null);
            return datatable;
        }
    }
}