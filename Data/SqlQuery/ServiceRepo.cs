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

namespace WorkAppReactAPI.Data.SqlQuery
{
    public class ServiceRepo : IServiceRepo
    {

        private readonly WorkerServiceContext _context;
        public ServiceRepo(WorkerServiceContext context)
        {
            _context = context;
        }

        public async Task<DynamicResult> getListService()
        {
            var result = await _context.ExecuteDataTable("[dbo].[sp_GetServices]", null).JsonDataAsync();
            return result;
        }
        public async Task<DynamicResult> AddService(ServiceUpdate model)
        {

            var service = await  _context.Services.FirstOrDefaultAsync(x => x.Name == model.Name.Trim());
            if (service != null)
            {
                var failure = new DynamicResult() { Message = "Name of service is exists", Type = "Error", Status = 2, Totalrow = 0 };
                return failure;
            }

            Guid? key;
            do
            {
                key = Guid.NewGuid();

            } while (await  _context.Services.FirstOrDefaultAsync(x => x.Id == key) != null);

            int number = 1;
            do
            {
                string code = DateTime.Now.ToString("ddMMyyhhmmss") + number;
                model.Code = "SVR" + code;
                number++;
            } while (await  _context.Services.FirstOrDefaultAsync(x => x.Code == model.Code) != null);



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

        public async Task<DynamicResult> UpdateService(ServiceUpdate model)
        {
            var service = _context.Services.FirstOrDefault(x => x.Code == model.Code);
            if (service == null)
            {
                var failure = new DynamicResult() { Message = "Not found service", Type = "Error", Status = 2, Totalrow = 0 };
                return failure;
            }
            SqlParameter[] parameters ={
                new SqlParameter("@Name", SqlDbType.NVarChar) { Value = model.Name},
                new SqlParameter("@ImageUrl", SqlDbType.VarChar) { Value = model.ImageUrl},
                new SqlParameter("@Description", SqlDbType.VarChar) { Value = model.Description},
            };
            var result = await _context.ExecuteDataTable("[dbo].[sp_UpdateService]", parameters).JsonDataAsync();
            return result;
        }
        public async Task<DynamicResult> DeleteService(ServiceDrop model)
        {
            var service = _context.Services.FirstOrDefault(x => x.Code == model.Code);
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

    }
}