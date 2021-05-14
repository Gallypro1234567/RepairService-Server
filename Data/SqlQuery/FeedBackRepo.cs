using System;
using System.Collections.Generic;
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
    public class FeedBackRepo : IFeedBackRepo
    {
        private readonly WorkerServiceContext _context;
        public FeedBackRepo(WorkerServiceContext context)
        {
            _context = context;
        }
        public async Task<DynamicResult> getFeedBackByWofSCode(FeedbackQuery query)
        {
            SqlParameter[] parameters ={
                new SqlParameter("@Wofscode", SqlDbType.VarChar) { Value = query.wofscode},
                new SqlParameter("@start", SqlDbType.Int) { Value = query.start},
                new SqlParameter("@length", SqlDbType.Int) { Value = query.length}
            };
            var result = await _context.ExecuteDataTable("[dbo].[sp_GetFeedBackbyCode]", parameters).JsonDataAsync();
            return result;

        }
         public async Task<DynamicResult> getWorkerRating(string wofscode)
        {
             SqlParameter[] parameters ={
                new SqlParameter("@Wofscode", SqlDbType.VarChar) { Value =  wofscode} 
            };
            var result = await _context.ExecuteDataTable("[dbo].[sp_GetFeedBackWorkerDetailbyCode]", parameters).JsonDataAsync();
            return result;
        }
        public async Task<DynamicResult> AddFeedBack(FeedbackUpdate model, UserLogin auth)
        {
            try
            {
                var wofs = await _context.WorkerOfServices.FirstOrDefaultAsync(x => x.Code == model.workerofservicecode);
                if (wofs == null)
                    return new DynamicResult() { Message = "Not Found WofSCode", Data = null, Totalrow = 0, Type = "Error", Status = 2 };
                var post = await _context.Posts.Include(x => x.Customer.User).FirstOrDefaultAsync(x => x.Code == model.postcode && x.Customer.User.Phone == auth.Phone);
                if (post == null)
                    return new DynamicResult() { Message = "Not found your post", Data = null, Totalrow = 0, Type = "Error-Authorized", Status = 2 };
                //
                Guid? key;
                string code;
                do
                {
                    key = Guid.NewGuid();

                } while (await _context.Feedbacks.FirstOrDefaultAsync(x => x.Id == key) != null);

                int number = 1;
                //
                do
                {
                    string temp = DateTime.Now.ToString("ddMMyyhhmmss") + number;
                    code = "FB" + temp;
                    number++;
                } while (await _context.Feedbacks.FirstOrDefaultAsync(x => x.Code == code) != null);

                SqlParameter[] parameters ={
                    new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = key},
                    new SqlParameter("@Code", SqlDbType.VarChar) { Value = code},
                    new SqlParameter("@WorkerOfServiceCode", SqlDbType.VarChar) { Value = model.workerofservicecode},
                    new SqlParameter("@PostCode", SqlDbType.VarChar) { Value = model.postcode},
                    new SqlParameter("@Description", SqlDbType.NVarChar) { Value = model.description},
                    new SqlParameter("@PointRating", SqlDbType.Float) { Value = model.pointRating},
                    new SqlParameter("@CreateAt", SqlDbType.DateTime) { Value = DateTime.Now},
                };
                var result = await _context.ExecuteDataTable("[dbo].[sp_InsertFeedBack]", parameters).JsonDataAsync();
                return result;
            }
            catch (System.Exception ex)
            {

                return new DynamicResult() { Message = ex.Message, Data = null, Totalrow = 0, Type = "Error-Exception", Status = 2 };

            }

        }

        public async Task<DynamicResult> DeleteFeedBack(string code, UserLogin auth)
        {
            try
            {
                var fb = await _context.Feedbacks.FirstOrDefaultAsync(x => x.Code == code);
                if (fb == null)
                    return new DynamicResult() { Message = "Not found FeedBacks", Data = null, Totalrow = 0, Type = "Error", Status = 2 };

                SqlParameter[] parameters ={
                    new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = fb.Id}
                };
                var result = await _context.ExecuteDataTable("[dbo].[sp_DeleteFeedBack]", parameters).JsonDataAsync();
                return result;
            }
            catch (System.Exception ex)
            {

                return new DynamicResult() { Message = ex.Message, Data = null, Totalrow = 0, Type = "Error-Exception", Status = 2 };

            }

        }

       
    }
}