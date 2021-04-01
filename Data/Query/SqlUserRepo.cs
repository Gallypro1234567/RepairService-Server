using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using WorkAppReactAPI.Assets;
using WorkAppReactAPI.Controllers;
using WorkAppReactAPI.Data.Interface;

namespace WorkAppReactAPI.Data
{
    public class SqlUserRepo : IUserRepo
    {

        private readonly WorkerServiceContext _context;
        public SqlUserRepo(WorkerServiceContext context)
        {
            _context = context;
        }

        public DynamicResult getAllUser()
        {
            
            var result = _context.ExecuteDataTable("[dbo].[sp_GetCustomers]", null).JsonData(); 
            return result;
        }

        public DynamicResult GetUserById(Guid id)
        {
           
            SqlParameter[] parameters ={
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = id},
            };
            var result = _context.ExecuteDataTable("[dbo].[sp_GetCustomerById]", parameters).JsonData();   
            return result;
        }
    }
}