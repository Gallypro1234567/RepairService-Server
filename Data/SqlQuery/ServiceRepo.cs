using System;
using System.Data;
using Microsoft.Data.SqlClient;
using WorkAppReactAPI.Assets;
using WorkAppReactAPI.Configuration;
using WorkAppReactAPI.Data.Interface;
using WorkAppReactAPI.Dtos.Requests;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkAppReactAPI.Dtos;

namespace WorkAppReactAPI.Data.SqlQuery
{
    public class ServiceRepo : IServiceRepo
    {

        private readonly WorkerServiceContext _context;
        public ServiceRepo(WorkerServiceContext context)
        {
            _context = context;
        }

        public async Task<DynamicResult> getListService(Query model)
        {
            SqlParameter[] parameters ={
                    new SqlParameter("@start", SqlDbType.Int) { Value = model.Start},
                    new SqlParameter("@length", SqlDbType.Int) { Value = model.Length},
                    new SqlParameter("@order", SqlDbType.Int) { Value = model.Order},
                    
                };
            var result = await _context.ExecuteDataTable("[dbo].[sp_GetServices]", parameters).JsonDataAsync();
            return result;
        }
        public async Task<DynamicResult> AddService(ServiceUpdate model, UserLogin auth)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(x => x.Phone == auth.Phone && x.Password ==  Encryptor.Encrypt(auth.Password));
                if (user == null)
                {
                    var failure = new DynamicResult() { Message = "Not found user", Type = "Error", Status = 2, Totalrow = 0 };
                    return failure;
                }
                if(user.Role > 0){
                    var failure = new DynamicResult() { Message = "You can't not add service", Type = "Error", Status = 2, Totalrow = 0 };
                    return failure;
                }

                var service = await _context.Services.FirstOrDefaultAsync(x => x.Name == model.Name.Trim());
                if (service != null)
                {
                    var failure = new DynamicResult() { Message = "Name of service is exists", Type = "Error", Status = 2, Totalrow = 0 };
                    return failure;
                }

                Guid? key;
                do
                {
                    key = Guid.NewGuid();

                } while (await _context.Services.FirstOrDefaultAsync(x => x.Id == key) != null);

                int number = 1;
                do
                {
                    string code = DateTime.Now.ToString("ddMMyyhhmmss") + number;
                    model.Code = "SVR" + code;
                    number++;
                } while (await _context.Services.FirstOrDefaultAsync(x => x.Code == model.Code) != null);



                SqlParameter[] parameters ={
                    new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = key},
                    new SqlParameter("@Code", SqlDbType.VarChar) { Value = model.Code},
                    new SqlParameter("@Name", SqlDbType.NVarChar) { Value = model.Name},
                    new SqlParameter("@ImageUrl", SqlDbType.VarChar) { Value = model.ImageUrl != null ?  model.ImageUrl : ""},
                    new SqlParameter("@Description", SqlDbType.VarChar) { Value = model.Description != null ?  model.Description : ""},
                    new SqlParameter("@CreateAt", SqlDbType.DateTime) { Value = DateTime.Now},
                };
                var result = await _context.ExecuteDataTable("[dbo].[sp_InsertService]", parameters).JsonDataAsync();
                return result;
            }
            catch (Exception ex)
            {
                return new DynamicResult()
                {
                    Message = ex.Message,
                    Status = 2,
                    Type = "Error"
                };
            }
        }

        public async Task<DynamicResult> UpdateService(string code, ServiceUpdate model, UserLogin auth)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(x => x.Phone == auth.Phone && x.Password ==  Encryptor.Encrypt(auth.Password));
                if (user == null)
                {
                    var failure = new DynamicResult() { Message = "Not found user", Type = "Error", Status = 2, Totalrow = 0 };
                    return failure;
                }
                if(user.Role > 0){
                    var failure = new DynamicResult() { Message = "You can't not update service", Type = "Error", Status = 2, Totalrow = 0 };
                    return failure;
                }

                var service = _context.Services.FirstOrDefault(x => x.Code == code);
                if (service == null)
                {
                    var failure = new DynamicResult() { Message = "Not found service", Type = "Error", Status = 2, Totalrow = 0 };
                    return failure;
                }
                SqlParameter[] parameters ={
                    new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = service.Id},
                    new SqlParameter("@Name", SqlDbType.NVarChar) { Value = model.Name},
                    new SqlParameter("@ImageUrl", SqlDbType.VarChar) { Value = model.ImageUrl == null ? "": model.ImageUrl},
                    new SqlParameter("@Description", SqlDbType.VarChar) { Value = model.Description == null ?"" : model.Description },
                };
                var result = await _context.ExecuteDataTable("[dbo].[sp_UpdateService]", parameters).JsonDataAsync();
                if(result.Status == 1 && service.ImageUrl.Length > 0){
                     System.IO.File.Delete(service.ImageUrl);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new DynamicResult()
                {
                    Message = ex.Message,
                    Status = 2,
                    Type = "Error"
                };
            }
        }
        public async Task<DynamicResult> DeleteService(string code,  UserLogin auth)
        {
            try
            {
                var service = _context.Services.FirstOrDefault(x => x.Code == code);
                if (service == null)
                {
                    var failure = new DynamicResult() { Message = "Not found service", Type = "Error", Status = 2, Totalrow = 0 };
                    return failure;
                }
                SqlParameter[] parameters ={
                    new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = service.Id},
                };
                var result = await _context.ExecuteDataTable("[dbo].[sp_DeleteServiceById]", parameters).JsonDataAsync();
                return result;
            }
            catch (Exception ex)
            {
                return new DynamicResult()
                {
                    Message = ex.Message,
                    Status = 2,
                    Type = "Error"
                };
            }
        }

    
    }
}