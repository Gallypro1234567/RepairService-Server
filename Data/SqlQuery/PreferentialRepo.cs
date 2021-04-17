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
    class PreferentialRepo : IPreferentialRepo
    {
        private readonly WorkerServiceContext _context;
        public PreferentialRepo(WorkerServiceContext context)
        {
            _context = context;
        }

        public async Task<DynamicResult> AddPreferential(PreferentialUpdate model)
        {
            var result = new DynamicResult();
            String[] prfs = model.ListService.Split(new char[] { ',' });

            var preferentials = await _context.Preferentials.FirstOrDefaultAsync(x => x.Title == model.Title.Trim());

            if (preferentials != null)
            {
                var failure = new DynamicResult() { Message = "Name of preferential is exists", Type = "Error", Status = 2, Totalrow = 0 };
                return failure;
            }
            foreach (var item in prfs)
            {

                var service = await _context.Services.FirstOrDefaultAsync(x => x.Code == item);
                if (service == null)
                {
                    var failure = new DynamicResult() { Message = "Code of service is not found", Type = "Error", Status = 2, Totalrow = 0 };
                    return failure;
                }
            }

            //------Paramaters
            Guid? key;
            do
            {
                key = Guid.NewGuid();

            } while (await _context.Preferentials.FirstOrDefaultAsync(x => x.Id == key) != null);

            int number = 1;
            do
            {
                key = Guid.NewGuid();
                string code = DateTime.Now.ToString("ddMMyyhhmmss") + number;
                model.Code = "PRF" + code;
                number++;

            } while (await _context.Preferentials.FirstOrDefaultAsync(x => x.Code == model.Code) != null);

            SqlParameter[] parameters ={
                    new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = key},
                    new SqlParameter("@Code", SqlDbType.VarChar) { Value = model.Code},
                    new SqlParameter("@Title", SqlDbType.NVarChar) { Value = model.Title},
                    new SqlParameter("@ImageUrl", SqlDbType.VarChar) { Value = model.ImageUrl},
                    new SqlParameter("@Description", SqlDbType.NVarChar) { Value = model.Description},
                    new SqlParameter("@Percent", SqlDbType.Float) { Value = model.Percents},
                    new SqlParameter("@FromDate", SqlDbType.DateTime) { Value = model.FromDate},
                    new SqlParameter("@ToDate", SqlDbType.DateTime) { Value = model.ToDate}

                };

            var result1 = await _context.ExecuteDataTable("[dbo].[sp_InsertPreferential]", parameters).JsonDataAsync();
            var result2 = new DynamicResult();

            foreach (var item in prfs)
            {
                var service = await _context.Services.FirstOrDefaultAsync(x => x.Code == item);
                SqlParameter[] parameterss ={
                            new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = Guid.NewGuid()},
                            new SqlParameter("@PreferentialID", SqlDbType.UniqueIdentifier) { Value = key},
                            new SqlParameter("@ServiceId", SqlDbType.UniqueIdentifier) { Value = service.Id},
                        };
                result2 = await _context.ExecuteDataTable("[dbo].[sp_InsertPreferentialService]", parameterss).JsonDataAsync();
            }
            if (result1.Status == 2)
            {
                return result1;
            }
            if (result2.Status == 2)
            {
                return result2;
            }
            return new DynamicResult()
            {
                Message = "Insert is Successed",
                Status = 2,
                Type = "Success"
            };

        }

        public async Task<DynamicResult> DeletePreferential(PreferentialDrop model)
        {
            var preferential = await _context.Preferentials.FirstOrDefaultAsync(x => x.Code == model.Code);
            if (preferential == null)
            {
                var failure = new DynamicResult() { Message = "Not found preferential", Type = "Error", Status = 2, Totalrow = 0 };
                return failure;
            }
            SqlParameter[] parameters ={
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = preferential.Id},
            };
            var result = await _context.ExecuteDataTable("[dbo].[sp_DeletePreferentialById]", parameters).JsonDataAsync();
            return result;
        }

        public async Task<DynamicResult> ListPreferentials()
        {
            var result = await _context.ExecuteDataTable("[dbo].[sp_GetPreferentials]", null).JsonDataAsync();
            return result;
        }

        public Task<DynamicResult> UpdatePreferential(PreferentialUpdate model)
        {
            throw new NotImplementedException();
        }
    }
}