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
    public class WorkerOfServicesRepo : IWorkerOfServicesRepo
    {
        private readonly WorkerServiceContext _context;
        public WorkerOfServicesRepo(WorkerServiceContext context)
        {
            _context = context;
        }

        public async Task<DynamicResult> GetallWorkerOfServices(Query model)
        {
            SqlParameter[] parameters ={
                new SqlParameter("@Code", SqlDbType.NVarChar) { Value = model.Code == null ? DBNull.Value :model.Code},
                new SqlParameter("@start", SqlDbType.Int) { Value = model.Start},
                new SqlParameter("@length", SqlDbType.Int) { Value = model.Length},
                new SqlParameter("@order", SqlDbType.Int) { Value = model.Order},
            };
            var result = await _context.ExecuteDataTable("[dbo].[sp_GetAllWorkerOfServices]", parameters).JsonDataAsync();
            return result;
        }

        public async Task<DynamicResult> RegisterWorkerOfServices(WorkerOfServicesUpdate model, UserLogin auth)
        {
            var result = new DynamicResult();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Phone == auth.Phone && x.Password == Encryptor.Encrypt(auth.Password));
            if (user == null)
            {
                return new DynamicResult() { Message = "User not found", Data = null, Totalrow = 0, Type = "Error", Status = 2 };

            }
            var checkWorker = await _context.Workers.FirstOrDefaultAsync(x => x.Id == user.Id);
            if (checkWorker == null)
            {
                return new DynamicResult() { Message = "User not found", Data = null, Totalrow = 0, Type = "Error", Status = 2 };
            }
            //
            Guid? key;
            do
            {
                key = Guid.NewGuid();

            } while (await _context.WorkerOfServices.FirstOrDefaultAsync(x => x.Id == key) != null);

            int number = 1;

            //
            do
            {
                string code = DateTime.Now.ToString("ddMMyyhhmmss") + number;
                model.WorkerOfServicesCode = "WOFS" + code;
                number++;
            } while (await _context.WorkerOfServices.FirstOrDefaultAsync(x => x.Code == model.WorkerOfServicesCode) != null);

            var service = await _context.Services.FirstOrDefaultAsync(x => x.Code == model.ServiceCode);
            if (service == null)
            {
                return new DynamicResult() { Message = "Service not found", Data = null, Totalrow = 0, Type = "Error", Status = 2 };

            }
            SqlParameter[] parameters ={
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = key},
                new SqlParameter("@Code", SqlDbType.VarChar) { Value = model.WorkerOfServicesCode},

                new SqlParameter("@WorkerID", SqlDbType.UniqueIdentifier) { Value = checkWorker.Id},
                new SqlParameter("@ServiceID", SqlDbType.UniqueIdentifier) { Value = service.Id},
                new SqlParameter("@FeelbackID", SqlDbType.UniqueIdentifier) { Value = DBNull.Value},

                new SqlParameter("@CreateAt", SqlDbType.DateTime) { Value = DateTime.Now},
                new SqlParameter("@isApproval", SqlDbType.Int) { Value = 0 },

                //Tạm thời
                new SqlParameter("@isOnline", SqlDbType.Bit) { Value = true},
                new SqlParameter("@Position", SqlDbType.VarChar) { Value = DBNull.Value},
            };

            result = await _context.ExecuteDataTable("[dbo].[sp_InsertWorkerOfService]", parameters).JsonDataAsync();
            return result;
        }

        public async Task<DynamicResult> VetificationWorkerOfServices(WorkerOfServicesUpdate model, UserLogin auth)
        {
            var result = new DynamicResult();
            var userAdmin = await _context.Users.FirstOrDefaultAsync(x => x.Phone == auth.Phone && x.Password == Encryptor.Encrypt(auth.Password));
            if (userAdmin == null)
            {
                return new DynamicResult() { Message = "Account already Exists", Data = null, Totalrow = 0, Type = "Error", Status = 2 };

            }
            if (userAdmin.Role != 0)
            {
                return new DynamicResult() { Message = "You are not Addmin", Data = null, Totalrow = 0, Type = "Error", Status = 2 };

            }
             
            var workerservice = await _context.WorkerOfServices.FirstOrDefaultAsync(x => x.Code == model.WorkerOfServicesCode);
            if (workerservice == null)
            {
                return new DynamicResult() { Message = "Service not found", Data = null, Totalrow = 0, Type = "Error", Status = 2 };

            } 

            SqlParameter[] parameters ={
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = workerservice.Id},
                new SqlParameter("@Code", SqlDbType.VarChar) { Value = workerservice.Code},


                // new SqlParameter("@ServiceID", SqlDbType.UniqueIdentifier) { Value = service.Id},
                new SqlParameter("@FeelbackID", SqlDbType.UniqueIdentifier) { Value = DBNull.Value},

                //new SqlParameter("@CreateAt", SqlDbType.DateTime) { Value = DateTime.Now},
                new SqlParameter("@isApproval", SqlDbType.Int) { Value = model.isApproval},
               
                //Tạm thời
                new SqlParameter("@isOnline", SqlDbType.Bit) { Value = true},
                new SqlParameter("@Position", SqlDbType.VarChar) { Value = DBNull.Value},
            };

            result = await _context.ExecuteDataTable("[dbo].[sp_UpdateWorkerOfService]", parameters).JsonDataAsync();
            return result;
        }

        public async Task<DynamicResult> DeleteWorkerOfServices(string ServiceCode, UserLogin auth)
        {
            var result = new DynamicResult();
            var userAdmin = await _context.Users.FirstOrDefaultAsync(x => x.Phone == auth.Phone && x.Password == Encryptor.Encrypt(auth.Password));
            if (userAdmin == null)
            {
                return new DynamicResult() { Message = "Account already Exists", Data = null, Totalrow = 0, Type = "Error", Status = 2 };

            }

            if (userAdmin.Role != 0)
            {
                return new DynamicResult() { Message = "You are not Addmin", Data = null, Totalrow = 0, Type = "Error", Status = 2 };

            }
            var workerservice = await _context.WorkerOfServices.FirstOrDefaultAsync(x => x.Code == ServiceCode);
            if (workerservice == null)
            {
                return new DynamicResult() { Message = "Service not found", Data = null, Totalrow = 0, Type = "Error", Status = 2 };

            }
            SqlParameter[] parameters ={

                new SqlParameter("@Id", SqlDbType.VarChar) { Value = workerservice.Id},

            };

            result = await _context.ExecuteDataTable("[dbo].[sp_DeletetWorkerOfService]", parameters).JsonDataAsync();
            return result;
        }


    }
}