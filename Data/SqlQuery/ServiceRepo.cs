using System;
using System.Data;
using Microsoft.Data.SqlClient;
using WorkAppReactAPI.Assets;
using WorkAppReactAPI.Configuration;
using WorkAppReactAPI.Data.Interface;
using WorkAppReactAPI.Dtos.Requests;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<DynamicResult> AddService(ServicePost model)
        {

            SqlParameter[] parameters ={
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = Guid.NewGuid().ToString()},
                new SqlParameter("@Code", SqlDbType.UniqueIdentifier) { Value = model.Code},
                new SqlParameter("@Name", SqlDbType.UniqueIdentifier) { Value = model.Name},
                new SqlParameter("@ImageUrl", SqlDbType.UniqueIdentifier) { Value = model.ImageUrl},
                new SqlParameter("@Description", SqlDbType.UniqueIdentifier) { Value = model.Description},
            };
            var result = await _context.ExecuteDataTable("[dbo].[sp_InsertService]", parameters).JsonDataAsync();
            return result;
        }

        public async Task<DynamicResult> UpdateService(ServicePost model)
        {
            var service = _context.Services.FirstOrDefault(x => x.Code == model.Code);
            if (service == null)
            {
                var failure = new DynamicResult() { Message = "Not found service", Type = "Error", Status = false, Totalrow = 0 };
                return failure;
            }
            SqlParameter[] parameters ={
                new SqlParameter("@Code", SqlDbType.UniqueIdentifier) { Value = model.Code},
                new SqlParameter("@Name", SqlDbType.UniqueIdentifier) { Value = model.Name},
                new SqlParameter("@ImageUrl", SqlDbType.UniqueIdentifier) { Value = model.ImageUrl},
                new SqlParameter("@Description", SqlDbType.UniqueIdentifier) { Value = model.Description},
            };
            var result = await _context.ExecuteDataTable("[dbo].[sp_UpdateService]", parameters).JsonDataAsync();
            return result;
        }
        public async Task<DynamicResult> DeleteService(ServicePost model)
        {
            var service = _context.Services.FirstOrDefault(x => x.Code == model.Code);
            if (service == null)
            {
                var failure = new DynamicResult() { Message = "Not found service", Type = "Error", Status = false, Totalrow = 0 };
                return failure;
            }
            SqlParameter[] parameters ={
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = service.Id},
            };
            var result =await _context.ExecuteDataTable("[dbo].[sp_DeleteServiceById]", parameters).JsonDataAsync();
            return result;
        }

    }
}