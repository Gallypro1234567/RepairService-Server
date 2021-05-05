using System;
using System.Data;
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
    public class WorkerRepo : IWorkerRepo
    {
        private readonly WorkerServiceContext _context;
        public WorkerRepo(WorkerServiceContext context)
        {
            _context = context;
        }
        public async Task<DynamicResult> getWorker(Query model)
        {
             SqlParameter[] parameters ={
                new SqlParameter("@start", SqlDbType.Decimal) { Value = model.Start},
                new SqlParameter("@length", SqlDbType.Int) { Value = model.Length},
                new SqlParameter("@order", SqlDbType.Int) { Value = model.Order}
            };
            var result = await _context.ExecuteDataTable("[dbo].[sp_GetWorkers]", parameters).JsonDataAsync();
            return result;
        }

        public async Task<DynamicResult> GetWorkerByPhone(string phone)
        {
            SqlParameter[] parameters ={
                new SqlParameter("@Phone", SqlDbType.VarChar) { Value = phone.Trim()}
            };
            var result = await _context.ExecuteDataTable("[dbo].[sp_GetWorkerByPhone]", parameters).JsonDataAsync();
            return result;
        }
        public async Task<DynamicResult> AddWorker(UserUpdate model)
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
            var result = await _context.ExecuteDataTable("[dbo].[sp_InsertWorker]", parameters).JsonDataAsync();
            return result;
        }

        public async Task<DynamicResult> UpdateWorker(string phone, UserUpdate model, UserLogin auth)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Phone == model.Phone && x.Phone == phone);
            if (user == null)
            {
                return new DynamicResult() { Message = "Account not found", Type = "Error", Status = 2, Data = null, Totalrow = 0 };

            }
            if (user.Phone != auth.Phone || user.Password != Encryptor.Encrypt(auth.Password))
            {
                return new DynamicResult() { Message = "You can't modify user's profile orther", Type = "Error-UnAuthorized", Status = 2, Data = null, Totalrow = 0 };

            }
            SqlParameter[] parameters ={
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = user.Id},
                new SqlParameter("@Phone", SqlDbType.VarChar) { Value = model.Phone},
                new SqlParameter("@Email", SqlDbType.VarChar) { Value = model.Email},
                new SqlParameter("@Fullname", SqlDbType.NVarChar) { Value = model.Fullname},
                new SqlParameter("@Sex", SqlDbType.Int) { Value = model.Sex},
                new SqlParameter("@Address", SqlDbType.DateTime) { Value = model.Address},
            };
            var result = await _context.ExecuteDataTable("[dbo].[sp_UpdateWorker]", parameters).JsonDataAsync();
            return result;
        }

        public async Task<DynamicResult> DeleteWorker(string phone, UserLogin auth)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Phone == phone);
            if (user == null)
            {
                return new DynamicResult() { Message = "Account not found", Type = "Error", Status = 2, Data = null, Totalrow = 0 };

            }
            SqlParameter[] parameters ={
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = user.Id}
            };
            var result = await _context.ExecuteDataTable("[dbo].[sp_DeleteWorker]", parameters).JsonDataAsync();
            return result;
        }
    }
}