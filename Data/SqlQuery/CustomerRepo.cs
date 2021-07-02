using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WorkAppReactAPI.Assets;
using WorkAppReactAPI.Configuration;
using WorkAppReactAPI.Data.Interface;
using WorkAppReactAPI.Dtos;
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
        public async Task<DynamicResult> getCustomer(Query model)
        {
            SqlParameter[] parameters ={
                new SqlParameter("@start", SqlDbType.Decimal) { Value = model.Start},
                new SqlParameter("@length", SqlDbType.Int) { Value = model.Length},
                new SqlParameter("@order", SqlDbType.Int) { Value = model.Order}
            };
            var result = await _context.ExecuteDataTable("[dbo].[sp_GetCustomers]", parameters).JsonDataAsync();
            return result;
        }

        public async Task<DynamicResult> GetCustomerByPhone(string phone)
        {
            SqlParameter[] parameters ={
                new SqlParameter("@Phone", SqlDbType.VarChar) { Value = phone.Trim()}
            };
            var result = await _context.ExecuteDataTable("[dbo].[sp_GetCustomerByPhone]", parameters).JsonDataAsync();
            return result;
        }
        public async Task<DynamicResult> AddCustomer(UserUpdate model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Phone == model.Phone);
            if (user != null)
            {
                return new DynamicResult() { Message = "Account already Exists", Data = null, Totalrow = 0, Type = "Error", Status = 2 };

            }
            SqlParameter[] parameters ={
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = Guid.NewGuid().ToString()},
                new SqlParameter("@Phone", SqlDbType.VarChar) { Value = model.Phone},
                new SqlParameter("@Email", SqlDbType.VarChar) { Value = model.Email},
                new SqlParameter("@Fullname", SqlDbType.NVarChar) { Value = model.Fullname},
                new SqlParameter("@Sex", SqlDbType.Int) { Value = model.Sex},
                new SqlParameter("@Address", SqlDbType.DateTime) { Value = model.Address},
            };
            var result = await _context.ExecuteDataTable("[dbo].[sp_InsertCustomer]", parameters).JsonDataAsync();
            return result;
        }

        public async Task<DynamicResult> UpdateCustomer(string phone, UserUpdate model, UserLogin auth)
        {
            var result = new DynamicResult();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Phone == phone && x.Phone == model.Phone);
            if (user == null)
            {

                return new DynamicResult() { Message = "Account not found", Data = null, Totalrow = 0, Type = "Error-Validation", Status = 2 };

            }
            if (user.Phone != auth.Phone && user.Password != Encryptor.Encrypt(auth.Password))
            {
                return new DynamicResult() { Message = "You can't modify  orther user's profile", Data = null, Totalrow = 0, Type = "Error-UnAuthorized", Status = 2 };
            }

            SqlParameter[] parameters1 = {
                    new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = user.Id },
                    new SqlParameter("@Phone", SqlDbType.VarChar) { Value = user.Phone},
                    new SqlParameter("@FullName", SqlDbType.NVarChar) { Value = model.Fullname},
                    new SqlParameter("@Sex", SqlDbType.Int) { Value = model.Sex},
                    new SqlParameter("@Email", SqlDbType.VarChar) { Value = model.Email},
                    new SqlParameter("@ImageUrl", SqlDbType.VarChar) { Value = model.ImageUrl},
                    new SqlParameter("@Address", SqlDbType.NVarChar) { Value = model.Address}
                };
            result = await _context.ExecuteDataTable("[dbo].[sp_UpdateCustomer]", parameters1).JsonDataAsync();
            return result;
        }

        public async Task<DynamicResult> DeleteCustomer(String phone, UserLogin auth)
        {
            var admin = await _context.Users.FirstOrDefaultAsync(x => x.Phone == auth.Phone && x.Password == Encryptor.Encrypt(auth.Password));
            if (admin == null)
            {
                return new DynamicResult() { Message = "Account not found ", Type = "Error", Status = 2, Data = null, Totalrow = 0 };

            }
            if (admin.Role != 0)
            {
                return new DynamicResult() { Message = "You are't admin", Type = "Error", Status = 2, Data = null, Totalrow = 0 };

            }
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Phone == phone);
            if (user == null)
            {
                return new DynamicResult() { Message = "Account not found", Type = "Error", Status = 2, Data = null, Totalrow = 0 };

            }
            SqlParameter[] parameters ={
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = user.Id}
            };
            var result = await _context.ExecuteDataTable("[dbo].[sp_DeleteCustomer]", parameters).JsonDataAsync();
            return result;
        }

        public async Task<DynamicResult> getinfo(string phone)
        {
            SqlParameter[] parameters ={
                new SqlParameter("@Phone", SqlDbType.VarChar) { Value = phone}
            };
            var result = await _context.ExecuteDataTable("[dbo].[sp_GetInfoOfUser]", parameters).JsonDataAsync();
            return result;
        }
    }
}