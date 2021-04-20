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
using WorkAppReactAPI.Models;

namespace WorkAppReactAPI.Data.SqlQuery
{
    class PreferentialRepo : IPreferentialRepo
    {
        private readonly WorkerServiceContext _context;
        public PreferentialRepo(WorkerServiceContext context)
        {
            _context = context;
        }

        public async Task<DynamicResult> AddPreferential(PreferentialUpdate model, UserLogin auth)
        {
            var result = new DynamicResult();
            try
            {
                var user = _context.Users.FirstOrDefault(x => x.Phone == auth.Phone && x.Password == Encryptor.Encrypt(auth.Password));
                if (user == null)
                {
                    var failure = new DynamicResult() { Message = "Not found user", Type = "Error", Status = 2, Totalrow = 0 };
                    return failure;
                }
                if (user.Role > 0)
                {
                    var failure = new DynamicResult() { Message = "You can't not insert preferential", Type = "Error", Status = 2, Totalrow = 0 };
                    return failure;
                }
                String[] prfs = model.ServiceCodes.Split(new char[] { ',' });

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
                    new SqlParameter("@ImageUrl", SqlDbType.VarChar) { Value = model.ImageUrl == null? DBNull.Value : model.ImageUrl},
                    new SqlParameter("@Description", SqlDbType.NVarChar) { Value = model.Description  == null? DBNull.Value : model.Description},
                    new SqlParameter("@Percent", SqlDbType.Float) { Value = model.Percents},
                    new SqlParameter("@FromDate", SqlDbType.DateTime) { Value = model.FromDate  == null? DBNull.Value : model.FromDate},
                    new SqlParameter("@ToDate", SqlDbType.DateTime) { Value = model.ToDate  == null? DBNull.Value : model.ToDate}

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
                    Message = "Insert is Successfully",
                    Status = 1,
                    Type = "Success"
                };
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

        public async Task<DynamicResult> DeletePreferential(string code, UserLogin auth)
        {
            var result = new DynamicResult();
            try
            {
                var user = _context.Users.FirstOrDefault(x => x.Phone == auth.Phone && x.Password == Encryptor.Encrypt(auth.Password));
                if (user == null)
                {
                    var failure = new DynamicResult() { Message = "Not found user", Type = "Error", Status = 2, Totalrow = 0 };
                    return failure;
                }
                if (user.Role > 0)
                {
                    var failure = new DynamicResult() { Message = "You can't not update service", Type = "Error", Status = 2, Totalrow = 0 };
                    return failure;
                }
                //
                var preferential = await _context.Preferentials.FirstOrDefaultAsync(x => x.Code == code);
                if (preferential == null)
                {
                    var failure = new DynamicResult() { Message = "Not found preferential", Type = "Error", Status = 2, Totalrow = 0 };
                    return failure;
                }
                var pref= await _context.Preferentials.FirstOrDefaultAsync(x => x.Code == code);
                var listprefservice = await _context.PreferentialOfServices.Where(x => x.Id == pref.Id).ToListAsync();
                foreach(var item in listprefservice){
                     PreferentialOfService itemdelete = await _context.PreferentialOfServices.FindAsync(item.Id);
                    _context.PreferentialOfServices.Remove(itemdelete);
                }
                _context.Preferentials.Remove(pref);
                 await  _context.SaveChangesAsync(); 
                 result.Start = 1;
                 result.Type = "Success";
                 result.Message ="Insert is successfully"; 
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

        public async Task<DynamicResult> ListPreferentials(Query model)
        {
            try
            {
                SqlParameter[] parameters ={
                    new SqlParameter("@start", SqlDbType.Int) { Value = model.Start},
                    new SqlParameter("@length", SqlDbType.Int) { Value = model.Length},
                    new SqlParameter("@order", SqlDbType.Int) { Value = model.Order},
                };
                var result = await _context.ExecuteDataTable("[dbo].[sp_GetPreferentials]", parameters).JsonDataAsync();
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

        public async Task<DynamicResult> UpdatePreferential(string code, PreferentialUpdate model, UserLogin auth)
        {
            var result = new DynamicResult();
            try
            {
                var user = _context.Users.FirstOrDefault(x => x.Phone == auth.Phone && x.Password == Encryptor.Encrypt(auth.Password));
                if (user == null)
                {
                    var failure = new DynamicResult() { Message = "Not found user", Type = "Error", Status = 2, Totalrow = 0 };
                    return failure;
                }
                if (user.Role > 0)
                {
                    var failure = new DynamicResult() { Message = "You can't not update preferential", Type = "Error", Status = 2, Totalrow = 0 };
                    return failure;
                }
                //


                String[] prfs = model.ServiceCodes.Split(new char[] { ',' });


                var preferential = await _context.Preferentials.FirstOrDefaultAsync(x => x.Code == code);

                if (preferential == null)
                {
                    var failure = new DynamicResult() { Message = "Preferential is not found", Type = "Error", Status = 2, Totalrow = 0 };
                    return failure;
                }
                var checkpreferential = await _context.Preferentials.Where(x => x.Title != model.Title.Trim()).ToListAsync();
                if (checkpreferential.Count() == 0 && checkpreferential.Count()  == 1 )
                {
                    var failure = new DynamicResult() { Message = "Preferential title was used", Type = "Error", Status = 2, Totalrow = 0 };
                    return failure;
                }
                foreach (var item in prfs)
                {

                    var service = await _context.Services.FirstOrDefaultAsync(x => x.Code == item);
                    if (service == null)
                    {
                        var failure = new DynamicResult() { Message = "Code service is not found", Type = "Error", Status = 2, Totalrow = 0 };
                        return failure;
                    }
                }

                SqlParameter[] parameters ={
                    new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = preferential.Id},
                    new SqlParameter("@Code", SqlDbType.VarChar) { Value = preferential.Code},
                    new SqlParameter("@Title", SqlDbType.NVarChar) { Value = model.Title},
                    new SqlParameter("@ImageUrl", SqlDbType.VarChar) { Value = model.ImageUrl == null ? DBNull.Value :  model.ImageUrl},
                    new SqlParameter("@Description", SqlDbType.NVarChar) { Value = model.Description == null ? DBNull.Value :  model.Description},
                    new SqlParameter("@Percent", SqlDbType.Float) { Value = model.Percents},
                    new SqlParameter("@FromDate", SqlDbType.DateTime) { Value = model.FromDate == null ? DBNull.Value :  model.FromDate},
                    new SqlParameter("@ToDate", SqlDbType.DateTime) { Value = model.ToDate == null ? DBNull.Value :  model.ToDate}

                };
  

                var listItem = await _context.PreferentialOfServices.Include(x => x.Preferential).Where(x => x.Preferential.Id == preferential.Id).ToListAsync();
                foreach (var item in listItem)
                {
                    PreferentialOfService itemdelete = await _context.PreferentialOfServices.FindAsync(item.Id);
                    _context.PreferentialOfServices.Remove(itemdelete);
                }
                await _context.SaveChangesAsync();
                // remove list old

                var result1 = await _context.ExecuteDataTable("[dbo].[sp_UpdatePreferential]", parameters).JsonDataAsync();
                var result2 = new DynamicResult();
                // insert new
                foreach (var item in prfs)
                {

                    var service = await _context.Services.FirstOrDefaultAsync(x => x.Code == item);
                    SqlParameter[] parameterss ={
                            new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = Guid.NewGuid()},
                            new SqlParameter("@PreferentialID", SqlDbType.UniqueIdentifier) { Value = preferential.Id},
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
                    Message = "Update is Successfully",
                    Status = 1,
                    Type = "Success"
                };
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