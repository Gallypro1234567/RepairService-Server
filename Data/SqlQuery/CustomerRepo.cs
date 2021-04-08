using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WorkAppReactAPI.Assets;
using WorkAppReactAPI.Configuration;
using WorkAppReactAPI.Data.Interface;
using WorkAppReactAPI.Dtos.Requests;

namespace WorkAppReactAPI.Data.SqlQuery
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly WorkerServiceContext _context;
        public CustomerRepo(WorkerServiceContext context)
        {
            _context = context;
        }
        public async Task<DynamicResult> getAllCustomer()
        {
            var result = await _context.ExecuteDataTable("[dbo].[sp_GetCustomers]", null).JsonDataAsync();
            return result;
        }

        public async Task<DynamicResult> GetCustomerByCode(string phone)
        {
            SqlParameter[] parameters ={
                new SqlParameter("@Phone", SqlDbType.UniqueIdentifier) { Value = phone.Trim()}  
            };
            var result =await _context.ExecuteDataTable("[dbo].[sp_GetCustomerByPhone]", parameters).JsonDataAsync();
            return result;
        } 
        public async Task<DynamicResult> AddCustomer(UserUpdate model)
        {
            var user =await _context.Users.FirstOrDefaultAsync(x => x.Phone == model.Phone);
            if (user != null)
            {
                return new DynamicResult() { Message = "Account already Exists", Data = null, Totalrow = 0, Type = "Error", Status = false };

            }
            SqlParameter[] parameters ={
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = Guid.NewGuid().ToString()},
                new SqlParameter("@Phone", SqlDbType.VarChar) { Value = model.Phone},
                new SqlParameter("@Email", SqlDbType.VarChar) { Value = model.Email},
                new SqlParameter("@Fullname", SqlDbType.NVarChar) { Value = model.Fullname},
                new SqlParameter("@Birthday", SqlDbType.DateTime) { Value = model.Birthday},
                new SqlParameter("@Address", SqlDbType.DateTime) { Value = model.Address},
            };
            var result =await _context.ExecuteDataTable("[dbo].[sp_InsertCustomer]", parameters).JsonDataAsync();
            return result;
        }

        public async Task<DynamicResult> UpdateCustomer(UserUpdate model)
        {
            var user =await _context.Users.FirstOrDefaultAsync(x => x.Phone == model.Phone);
            if (user == null)
            {
                return new DynamicResult() { Message = "Account not found", Type = "Error", Status = false, Data = null, Totalrow = 0 };

            }
            SqlParameter[] parameters ={
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = user.Id},
                new SqlParameter("@Phone", SqlDbType.VarChar) { Value = model.Phone},
                new SqlParameter("@Email", SqlDbType.VarChar) { Value = model.Email},
                new SqlParameter("@Fullname", SqlDbType.NVarChar) { Value = model.Fullname},
                new SqlParameter("@Birthday", SqlDbType.DateTime) { Value = model.Birthday},
                new SqlParameter("@Address", SqlDbType.DateTime) { Value = model.Address},
            };
            var result =await _context.ExecuteDataTable("[dbo].[sp_UpdateCustomer]", parameters).JsonDataAsync();
            return result;
        }

        public async Task<DynamicResult> DeleteCustomerById(UserUpdate model)
        {
            var user =await _context.Users.FirstOrDefaultAsync(x => x.Phone == model.Phone);
            if (user == null)
            {
                return new DynamicResult() { Message = "Account not found", Type = "Error", Status = false, Data = null, Totalrow = 0 };

            }
            SqlParameter[] parameters ={
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = user.Id} 
            };
            var result =await _context.ExecuteDataTable("[dbo].[sp_DeleteCustomer]", parameters).JsonDataAsync();
            return result;
        }
    }
}