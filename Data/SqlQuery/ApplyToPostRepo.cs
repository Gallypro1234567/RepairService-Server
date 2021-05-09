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
using WorkAppReactAPI.Dtos;
using WorkAppReactAPI.Dtos.Requests;

namespace WorkAppReactAPI.Data.SqlQuery
{
    public class ApplyToPostRepo : IApplyToPostRepo
    {
        private readonly WorkerServiceContext _context;
        public ApplyToPostRepo(WorkerServiceContext context)
        {
            _context = context;
        }
        public async Task<DynamicResult> getApplytoPostbyCode(string code)
        {

            SqlParameter[] parameters ={
                new SqlParameter("@PostCode", SqlDbType.VarChar) { Value = code}
            };
            var result = await _context.ExecuteDataTable("[dbo].[sp_GetApplyPostByPostCode]", parameters).JsonDataAsync();
            return result;
        }
        public async Task<DynamicResult> checkApplytoPostbyWorkerPhone(string code, UserLogin auth)
        {
            var worker = await _context.Workers.Include(x => x.WorkerOfCategories).Include(x => x.User).FirstOrDefaultAsync(x => x.User.Phone == auth.Phone && x.User.Password == Encryptor.Encrypt(auth.Password));
            if (worker == null)
            {

                return new DynamicResult() { Message = "Account not found", Data = null, Totalrow = 0, Type = "Error-Validation", Status = 2 };

            }
            var post = await _context.Posts.Include(x => x.Service).FirstOrDefaultAsync(x => x.Code == code);
            if (post == null)
            {
                return new DynamicResult() { Message = "Post not found", Data = null, Totalrow = 0, Type = "Error", Status = 2 };

            }
            var workerofservice = await _context.WorkerOfServices.Include(x => x.Worker.User).Include(x => x.Service).FirstOrDefaultAsync(x => x.Worker.User.Phone == auth.Phone);
            if (workerofservice == null)
            {
                return new DynamicResult() { Message = "You can't apply post because service is not approval", Data = null, Totalrow = 0, Type = "Error-UnAuthorized", Status = 2 };
            }

            var applypost = await _context.ApplyToPosts.FirstOrDefaultAsync(x => x.WorkerOfServiceCode == workerofservice.Code && x.PostCode == code);
            var resp = new List<Dictionary<string, object>>();
            resp.Add(new Dictionary<string, object>()
            {
                ["WorkerOfServiceCode"] = workerofservice.Code,
                ["PostCode"] = code,
                ["Status"] = applypost == null ? null : applypost.Status.ToString(),
            });

            return new DynamicResult() { Message = "Checked", Data = resp, Totalrow = 0, Type = "Success", Status = 1 };

        }
        public async Task<DynamicResult> getApplytoPostbyWorkerPhone(string phone)
        {
            SqlParameter[] parameters ={
                new SqlParameter("@WorkerPhone", SqlDbType.VarChar) { Value = phone}
            };
            var result = await _context.ExecuteDataTable("[dbo].[sp_GetApplyPostByWorkerPhone]", parameters).JsonDataAsync();
            return result;
        }


        public async Task<DynamicResult> AddApplytoPost(ApplyToPostUpdate model, UserLogin auth)
        {
            var result = new DynamicResult();
            var worker = await _context.Workers.Include(x => x.WorkerOfCategories).Include(x => x.User).FirstOrDefaultAsync(x => x.User.Phone == auth.Phone && x.User.Password == Encryptor.Encrypt(auth.Password));
            if (worker == null)
            {

                return new DynamicResult() { Message = "Account not found", Data = null, Totalrow = 0, Type = "Error-Validation", Status = 2 };

            }
            var post = await _context.Posts.Include(x => x.Service).FirstOrDefaultAsync(x => x.Code == model.postcode);
            if (post == null)
            {
                return new DynamicResult() { Message = "Post not found", Data = null, Totalrow = 0, Type = "Error", Status = 2 };

            }
            var workerofservice = await _context.WorkerOfServices.Include(x => x.Worker.User).Include(x => x.Service).FirstOrDefaultAsync(x => x.Worker.User.Phone == auth.Phone && x.Service.Code == post.Service.Code);
            if (workerofservice == null)
            {
                return new DynamicResult() { Message = "You can't apply post because service is not approval", Data = null, Totalrow = 0, Type = "Error-UnAuthorized", Status = 2 };
            }

            var applypost = await _context.ApplyToPosts.FirstOrDefaultAsync(x => x.WorkerOfServiceCode == workerofservice.Code && x.PostCode == model.postcode);
            if (applypost != null)
            {
                return new DynamicResult() { Message = "You used to apply this post", Data = null, Totalrow = 0, Type = "Error", Status = 2 };
            }
            Guid? key;
            do
            {
                key = Guid.NewGuid();

            } while (await _context.Posts.FirstOrDefaultAsync(x => x.Id == key) != null);

            SqlParameter[] parameters ={
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = key},
                new SqlParameter("@WorkerOfServiceCode", SqlDbType.VarChar) { Value = workerofservice.Code},
                new SqlParameter("@PostCode", SqlDbType.VarChar) { Value = model.postcode},
                new SqlParameter("@Status", SqlDbType.Int) { Value = model.status},
                new SqlParameter("@AcceptAt", SqlDbType.DateTime) { Value = DateTime.Now},
                new SqlParameter("@CreateAt", SqlDbType.DateTime) { Value = DateTime.Now}
            };
            result = await _context.ExecuteDataTable("[dbo].[sp_InsertApplyPost]", parameters).JsonDataAsync();
            return result;
        }
        public async Task<DynamicResult> UpdateApplytoPost(ApplyToPostUpdate model, UserLogin auth)
        {
            var customer = await _context.Customers.Include(x => x.User).FirstOrDefaultAsync(x => x.User.Phone == auth.Phone && x.User.Password == Encryptor.Encrypt(auth.Password));
            if (customer == null)
            {

                return new DynamicResult() { Message = "Account not found", Data = null, Totalrow = 0, Type = "Error-Validation", Status = 2 };

            }
            var postofUser = await _context.Posts.FirstOrDefaultAsync(x => x.Code == model.postcode);
            if (postofUser == null)
            {
                return new DynamicResult() { Message = "Post not found", Data = null, Totalrow = 0, Type = "Error-UnAuthorized", Status = 2 };
            }
            var applypost = await _context.ApplyToPosts.FirstOrDefaultAsync(x => x.WorkerOfServiceCode == model.workerofservice && x.PostCode == model.postcode);
            if (applypost == null)
            {
                return new DynamicResult() { Message = "Your Apply not found", Data = null, Totalrow = 0, Type = "Error-UnAuthorized", Status = 2 };
            }
            SqlParameter[] parameters ={
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = applypost.Id},
                new SqlParameter("@WorkerOfServiceCode", SqlDbType.VarChar) { Value = applypost.WorkerOfServiceCode},
                new SqlParameter("@PostCode", SqlDbType.VarChar) { Value = applypost.PostCode},
                new SqlParameter("@Status", SqlDbType.Int) { Value = model.status},
                new SqlParameter("@AcceptAt", SqlDbType.DateTime) { Value = DateTime.Now},
                new SqlParameter("@CreateAt", SqlDbType.DateTime) { Value = applypost.CreateAt}
            };
            var result = await _context.ExecuteDataTable("[dbo].[sp_UpdateApplyPost]", parameters).JsonDataAsync();
            return result;
        }
        public async Task<DynamicResult> customerAcceptPostApply(ApplyToPostUpdate model, UserLogin auth)
        {
            var customer = await _context.Customers.Include(x => x.User).FirstOrDefaultAsync(x => x.User.Phone == auth.Phone && x.User.Password == Encryptor.Encrypt(auth.Password));
            if (customer == null)
            {

                return new DynamicResult() { Message = "Account not found", Data = null, Totalrow = 0, Type = "Error-Validation", Status = 2 };

            }
            var postofUser = await _context.Posts.FirstOrDefaultAsync(x => x.Code == model.postcode);
            if (postofUser == null)
            {
                return new DynamicResult() { Message = "Post not found", Data = null, Totalrow = 0, Type = "Error", Status = 2 };
            }
            var wofs = await _context.WorkerOfServices.FirstOrDefaultAsync(x => x.Code == model.workerofservice);
            if (wofs == null)
            {
                return new DynamicResult() { Message = "Not found", Data = null, Totalrow = 0, Type = "Error", Status = 2 };
            }
            SqlParameter[] parameters ={

                    new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = postofUser.Id},
                    new SqlParameter("@WorkerOfServiceID", SqlDbType.UniqueIdentifier) { Value = wofs.Id},
                    new SqlParameter("@Status", SqlDbType.Int) { Value = model.status},
                };

            var result = await _context.ExecuteDataTable("[dbo].[sp_UpdatePostByAccept]", parameters).JsonDataAsync();
            return result;
        }

        public async Task<DynamicResult> DeleteApplytoPost(ApplyToPostUpdate model, UserLogin auth)
        {
             
            var worker = await _context.Workers.Include(x => x.WorkerOfCategories).Include(x => x.User).FirstOrDefaultAsync(x => x.User.Phone == auth.Phone && x.User.Password == Encryptor.Encrypt(auth.Password));
            if (worker == null)
            {
                return new DynamicResult() { Message = "Account not found", Data = null, Totalrow = 0, Type = "Error-Validation", Status = 2 };
            }
            var post = await _context.Posts.Include(x => x.Service).FirstOrDefaultAsync(x => x.Code == model.postcode);
            if (post == null)
            {
                return new DynamicResult() { Message = "Post not found", Data = null, Totalrow = 0, Type = "Error", Status = 2 };

            }
            var workerofservice = await _context.WorkerOfServices.Include(x => x.Worker.User).Include(x => x.Service).FirstOrDefaultAsync(x => x.Worker.User.Phone == auth.Phone && x.Service.Code == post.Service.Code);
            if (workerofservice == null)
            {
                return new DynamicResult() { Message = "Not found", Data = null, Totalrow = 0, Type = "Error", Status = 2 };
            }
            var applypost = await _context.ApplyToPosts.FirstOrDefaultAsync(x => x.WorkerOfServiceCode == workerofservice.Code && x.PostCode == model.postcode);
            if (applypost == null)
            {
                return new DynamicResult() { Message = " Apply not found", Data = null, Totalrow = 0, Type = "Error", Status = 2 };
            }

            SqlParameter[] parameters ={
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = applypost.Id},
                new SqlParameter("@WorkerOfServiceCode", SqlDbType.VarChar) { Value = applypost.WorkerOfServiceCode},
                new SqlParameter("@PostCode", SqlDbType.VarChar) { Value = applypost.PostCode},
                new SqlParameter("@Status", SqlDbType.Int) { Value = -1},
                new SqlParameter("@AcceptAt", SqlDbType.DateTime) { Value = DateTime.Now},
                new SqlParameter("@CreateAt", SqlDbType.DateTime) { Value = applypost.CreateAt}
            };
            var result = await _context.ExecuteDataTable("[dbo].[sp_UpdateApplyPost]", parameters).JsonDataAsync();
            return result;
        }


    }
}