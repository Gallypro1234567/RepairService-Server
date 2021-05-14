using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WorkAppReactAPI.Assets;
using WorkAppReactAPI.Configuration;
using WorkAppReactAPI.Data.Interface;
using WorkAppReactAPI.Dtos.Requests;

namespace WorkAppReactAPI.Data.SqlQuery
{
    public class PostRepo : IPostRepo
    {
        private readonly WorkerServiceContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public PostRepo(WorkerServiceContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _hostingEnvironment = webHostEnvironment;
        }

        public async Task<DynamicResult> GetPost(PostGet model)
        {
            SqlParameter[] parameters ={
                new SqlParameter("@Code", SqlDbType.VarChar) { Value = model.ServiceCode == null ? DBNull.Value : model.ServiceCode },
                new SqlParameter("@start", SqlDbType.Int) { Value = model.Start},
                new SqlParameter("@length", SqlDbType.Int) { Value = model.Length},
                new SqlParameter("@order", SqlDbType.Int) { Value = model.Order},
                new SqlParameter("@status", SqlDbType.Int) { Value = model.Status == null ? DBNull.Value : model.Status},
            };
            var result = await _context.ExecuteDataTable("[dbo].[sp_GetAllPosts]", parameters).JsonDataAsync();
            return result;
        }
        public async Task<DynamicResult> GetPostDetail(string Code)
        {
            SqlParameter[] parameters ={
                new SqlParameter("@Code", SqlDbType.VarChar) { Value =   Code},

            };
            var result = await _context.ExecuteDataTable("[dbo].[sp_GetPostDetailbyCode]", parameters).JsonDataAsync();
            return result;
        }
        public async Task<DynamicResult> GetRecentlyPosts(PostGet model)
        {
            SqlParameter[] parameters ={
                new SqlParameter("@start", SqlDbType.Int) { Value = model.Start},
                new SqlParameter("@length", SqlDbType.Int) { Value = model.Length},
                new SqlParameter("@order", SqlDbType.Int) { Value = model.Order},
                new SqlParameter("@status", SqlDbType.Int) { Value = model.Status == null ? DBNull.Value : model.Status},
            };
            var result = await _context.ExecuteDataTable("[dbo].[sp_GetRecentlyPosts]", parameters).JsonDataAsync();
            return result;
        }


        public async Task<DynamicResult> GetPostByPhone(string phone, PostGet model)
        {
            var result = new DynamicResult();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Phone == phone);
            if (user == null)
            {
                return new DynamicResult() { Message = "Account already Exists", Data = null, Totalrow = 0, Type = "Error", Status = 2 };

            }
            var isCustomer = await _context.Customers.Include(x => x.User).FirstOrDefaultAsync(x => x.User.Id == user.Id);
            SqlParameter[] parameters1 ={
                    new SqlParameter("@Phone", SqlDbType.VarChar) { Value = phone},
                    new SqlParameter("@start", SqlDbType.Int) { Value = model.Start},
                    new SqlParameter("@length", SqlDbType.Int) { Value = model.Length},
                    new SqlParameter("@order", SqlDbType.Int) { Value = model.Order},
                    new SqlParameter("@status", SqlDbType.Int) { Value = model.Status == null ? DBNull.Value : model.Status},
                };
            SqlParameter[] parameters2 ={
                    new SqlParameter("@Phone", SqlDbType.VarChar) { Value = phone},
                    new SqlParameter("@WorkerOfServiceCode", SqlDbType.VarChar) { Value = model.WofSCode == null ? DBNull.Value : model.WofSCode},
                    new SqlParameter("@start", SqlDbType.Int) { Value = model.Start},
                    new SqlParameter("@length", SqlDbType.Int) { Value = model.Length},
                    new SqlParameter("@order", SqlDbType.Int) { Value = model.Order},
                    new SqlParameter("@status", SqlDbType.Int) { Value =  model.Status == null ? DBNull.Value : model.Status},
                };

            if (isCustomer != null)
            {
                result = await _context.ExecuteDataTable("[dbo].[sp_GetAllPostsByCustomer]", parameters1).JsonDataAsync();
                return result;
            }

            result = await _context.ExecuteDataTable("[dbo].[sp_GetAllPostsByWorker]", parameters2).JsonDataAsync();
            return result;
        }
        public async Task<DynamicResult> InserPost(PostUpdate model, UserLogin auth)
        {
            var result = new DynamicResult();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Phone == auth.Phone && x.Password == Encryptor.Encrypt(auth.Password));
            if (user == null)
            {
                return new DynamicResult() { Message = "Account already Exists", Data = null, Totalrow = 0, Type = "Error", Status = 2 };

            }
            var service = await _context.Services.FirstOrDefaultAsync(x => x.Code == model.ServiceCode);
            if (service == null)
            {
                return new DynamicResult() { Message = "Not found Service", Data = null, Totalrow = 0, Type = "Error", Status = 2 };

            }
            var isCustomer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == user.Id);
            if (isCustomer == null)
            {
                return new DynamicResult() { Message = "Account already Exists", Data = null, Totalrow = 0, Type = "Error", Status = 2 };
            }
            //
            Guid? key;
            do
            {
                key = Guid.NewGuid();

            } while (await _context.Posts.FirstOrDefaultAsync(x => x.Id == key) != null);

            int number = 1;
            //
            do
            {
                string code = DateTime.Now.ToString("ddMMyyhhmmss") + number;
                model.Code = "PT" + code;
                number++;
            } while (await _context.Posts.FirstOrDefaultAsync(x => x.Code == model.Code) != null);


            SqlParameter[] parameters ={
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = key},
                new SqlParameter("@Code", SqlDbType.VarChar) { Value = model.Code},
                new SqlParameter("@CreateAt", SqlDbType.DateTime) { Value = DateTime.Now},
                //
                new SqlParameter("@CustomerID", SqlDbType.UniqueIdentifier) { Value = isCustomer.Id},
                new SqlParameter("@WorkerOfServiceID", SqlDbType.UniqueIdentifier) { Value = DBNull.Value},
                new SqlParameter("@ServiceID", SqlDbType.UniqueIdentifier) { Value = service.Id},
                //
                new SqlParameter("@Title", SqlDbType.NVarChar) { Value = model.Title},
                new SqlParameter("@DistrictId", SqlDbType.Int) { Value = model.DistrictId},
                new SqlParameter("@CityId", SqlDbType.Int) { Value = model.CityId},
                new SqlParameter("@Description", SqlDbType.NVarChar) { Value = model.Description == null ? DBNull.Value : model.Description},
                new SqlParameter("@Address", SqlDbType.NVarChar) { Value = model.Address  == null ? DBNull.Value: model.Address},
                new SqlParameter("@ImageUrl", SqlDbType.VarChar) { Value = model.ImageUrl == null ? DBNull.Value: model.ImageUrl},
                new SqlParameter("@FinishAt", SqlDbType.DateTime) { Value = model.FinishAt},
                //
                new SqlParameter("@status", SqlDbType.Int) { Value = model.status},
            };
            result = await _context.ExecuteDataTable("[dbo].[sp_InsertPost]", parameters).JsonDataAsync();
            return result;
        }

        public async Task<DynamicResult> UpdatePostByCustomer(string code, PostUpdate model, UserLogin auth)
        {
            var result = new DynamicResult();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Phone == auth.Phone && x.Password == Encryptor.Encrypt(auth.Password));
            if (user == null)
            {
                return new DynamicResult() { Message = "Account already Exists", Data = null, Totalrow = 0, Type = "Error", Status = 2 };

            }
            var isCustomer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == user.Id);
            if (isCustomer == null)
            {
                return new DynamicResult() { Message = "Account already Exists", Data = null, Totalrow = 0, Type = "Error", Status = 2 };
            }
            var service = await _context.Services.FirstOrDefaultAsync(x => x.Code == model.ServiceCode);
            if (service == null)
            {
                return new DynamicResult() { Message = "Not found Service", Data = null, Totalrow = 0, Type = "Error", Status = 2 };

            }
            var post = await _context.Posts.FirstOrDefaultAsync(x => x.Code == code);
            if (post == null)
            {
                return new DynamicResult() { Message = "Post not found", Data = null, Totalrow = 0, Type = "Error", Status = 2 };
            }
            var ImageUrlDelete = post.ImageUrl;
            // 
            SqlParameter[] parameters ={
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = post.Id},
                new SqlParameter("@Code", SqlDbType.VarChar) { Value = post.Code},
                new SqlParameter("@CreateAt", SqlDbType.DateTime) { Value = post.CreateAt},
                //
                new SqlParameter("@CustomerID", SqlDbType.UniqueIdentifier) { Value = post.Customer.Id},
                new SqlParameter("@WorkerOfServiceID", SqlDbType.UniqueIdentifier) { Value = DBNull.Value},
                 new SqlParameter("@ServiceID", SqlDbType.UniqueIdentifier) { Value = service.Id},
                //
                new SqlParameter("@Title", SqlDbType.NVarChar) { Value = model.Title},
                new SqlParameter("@DistrictId", SqlDbType.Int) { Value = model.DistrictId},
                new SqlParameter("@CityId", SqlDbType.Int) { Value = model.CityId},
                new SqlParameter("@Description", SqlDbType.NVarChar) { Value = model.Description == null ? DBNull.Value : model.Description},
                new SqlParameter("@Address", SqlDbType.NVarChar) { Value = model.Address  == null ? DBNull.Value: model.Address},
                new SqlParameter("@ImageUrl", SqlDbType.VarChar) { Value = model.ImageUrl == null ? DBNull.Value: model.ImageUrl},
                new SqlParameter("@FinishAt", SqlDbType.DateTime) { Value = model.FinishAt},
                //
                new SqlParameter("@status", SqlDbType.Int) { Value = model.status},
            };

            result = await _context.ExecuteDataTable("[dbo].[sp_UpdatePost]", parameters).JsonDataAsync();
            if (result.Status == 1 && ImageUrlDelete != null)
            {
                String[] imageurls = ImageUrlDelete.Split(new char[] { ',' });
                foreach (var item in imageurls)
                {
                    _hostingEnvironment.DeleteImage(item);
                }

            }
            return result;
        }

        public async Task<DynamicResult> UpdatePostByWorker(string code, UserLogin auth)
        {
            var result = new DynamicResult();
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Phone == auth.Phone && x.Password == Encryptor.Encrypt(auth.Password));
                if (user == null)
                {
                    return new DynamicResult() { Message = "Account already Exists", Data = null, Totalrow = 0, Type = "Error", Status = 2 };

                }

                var post = await _context.Posts.Include(x => x.Customer).Include(x => x.Service).Include(x => x.WorkerOfService).FirstOrDefaultAsync(x => x.Code == code);
                if (post == null)
                {
                    return new DynamicResult() { Message = "Post not found", Data = null, Totalrow = 0, Type = "Error", Status = 2 };
                }
                var service = await _context.Services.FirstOrDefaultAsync(x => x.Id == post.Service.Id);
                if (service == null)
                {
                    return new DynamicResult() { Message = "Post Not found Service", Data = null, Totalrow = 0, Type = "Error", Status = 2 };

                }
                var checkWofS = await _context.WorkerOfServices.Include(x => x.Worker).Include(x => x.Service).FirstOrDefaultAsync(x => x.Worker.Id == user.Id && x.Service.Id == service.Id);
                if (checkWofS == null || checkWofS.isApproval != 1)
                {
                    return new DynamicResult() { Message = "You can not apply this post", Data = null, Totalrow = 0, Type = "Error", Status = 2 };
                }

                // 
                SqlParameter[] parameters ={
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = post.Id},
                new SqlParameter("@Code", SqlDbType.VarChar) { Value = post.Code},
                new SqlParameter("@CreateAt", SqlDbType.DateTime) { Value = post.CreateAt},
                //
                new SqlParameter("@CustomerID", SqlDbType.UniqueIdentifier) { Value = post.Customer.Id},
                //
                new SqlParameter("@WorkerOfServiceID", SqlDbType.UniqueIdentifier) { Value = checkWofS.Id},
                new SqlParameter("@ServiceID", SqlDbType.UniqueIdentifier) { Value = post.Service.Id},
                //
                new SqlParameter("@Title", SqlDbType.NVarChar) { Value = post.Title},
                new SqlParameter("@DistrictId", SqlDbType.Int) { Value = post.DistrictId},
                new SqlParameter("@CityId", SqlDbType.Int) { Value = post.CityId},
                new SqlParameter("@Description", SqlDbType.NVarChar) { Value = post.Description == null ? DBNull.Value : post.Description},
                new SqlParameter("@Address", SqlDbType.NVarChar) { Value = post.Address == null ? DBNull.Value : post.Address},
                new SqlParameter("@ImageUrl", SqlDbType.VarChar) { Value = post.ImageUrl == null ? DBNull.Value : post.ImageUrl},
                new SqlParameter("@FinishAt", SqlDbType.DateTime) { Value = post.FinishAt},
                //
                new SqlParameter("@status", SqlDbType.Int) { Value = 1},
            };

                result = await _context.ExecuteDataTable("[dbo].[sp_UpdatePost]", parameters).JsonDataAsync();
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Status = 2;
                result.Type = "Error";
                return result;
            }


        }


        public async Task<DynamicResult> DeletePost(string postCode, UserLogin auth)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Phone == auth.Phone && x.Password == Encryptor.Encrypt(auth.Password));
            if (user == null)
            {
                return new DynamicResult() { Message = "Account already Exists", Data = null, Totalrow = 0, Type = "Error", Status = 2 };

            }
            var isCustomer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == user.Id);
            if (isCustomer == null)
            {
                return new DynamicResult() { Message = "Account already Exists", Data = null, Totalrow = 0, Type = "Error", Status = 2 };
            }
            var checkPost = await _context.Posts.FirstOrDefaultAsync(x => x.Code == postCode && x.Customer.Id == isCustomer.Id);
            var checkPostCode = checkPost.Code;
            if (checkPost == null)
            {
                return new DynamicResult() { Message = "You can't not delete post of orther user", Data = null, Totalrow = 0, Type = "Error", Status = 2 };
            }
            if (checkPost.status > 0)
            {
                return new DynamicResult() { Message = "You can't not delete post when it is processing", Data = null, Totalrow = 0, Type = "Error", Status = 2 };
            }
            var ImageUrlDelete = checkPost.ImageUrl;
            SqlParameter[] parameters ={
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = checkPost.Id}
            };
            var result = await _context.ExecuteDataTable("[dbo].[sp_DeletePost]", parameters).JsonDataAsync();
            if (result.Status == 1 && ImageUrlDelete != null)
            {
                String[] imageurls = ImageUrlDelete.Split(new char[] { ',' });
                foreach (var item in imageurls)
                {
                    _hostingEnvironment.DeleteImage(item);
                }

            }
            var applyPostList = await _context.ApplyToPosts.Where(x => x.PostCode == checkPostCode).ToListAsync();
            if (applyPostList.Count() != 0)
            {
                foreach (var applypost in applyPostList)
                {
                    SqlParameter[] parameters1 ={
                        new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = applypost.Id},
                    };
                    await _context.ExecuteDataTable("[dbo].[sp_DeleteApplyPost]", parameters1).JsonDataAsync();
                }
            }

            return result;
        }

        public async Task<DynamicResult> GetPostDetailFeeback(string postCode)
        {
            var post = await _context.Posts.Include(x => x.Service).Include(x => x.Customer.User).Include(x => x.WorkerOfService.Worker.User).Include(x => x.Service).FirstOrDefaultAsync(x => x.Code == postCode);
            if (post == null)
                return new DynamicResult() { Message = "Post not Found", Data = null, Totalrow = 0, Type = "Error", Status = 2 };
            var apply = await _context.ApplyToPosts.FirstOrDefaultAsync(x => x.PostCode == post.Code && x.WorkerOfServiceCode == post.WorkerOfService.Code);
            if (apply == null)
                return new DynamicResult() { Message = "Apply not Found", Data = null, Totalrow = 0, Type = "Error", Status = 2 };
            var fb = await _context.Feedbacks.FirstOrDefaultAsync(x => x.PostCode == postCode && x.WorkerOfServiceCode == post.WorkerOfService.Code);
            var resp = new List<Dictionary<string, object>>();
            resp.Add(new Dictionary<string, object>()
            {
                ["CustomerFullname"] = post.Customer.User.Fullname,
                ["CustomerPhone"] = post.Customer.User.Phone,
                ["CustomerAddress"] = post.Customer.User.Address,

                ["WorkerFullname"] = post.WorkerOfService.Worker.User.Fullname,
                ["WorkerPhone"] = post.WorkerOfService.Worker.User.Phone,
                ["WorkerAddress"] = post.WorkerOfService.Worker.User.Address,
                ["WorkerCMND"] = post.WorkerOfService.Worker.CMND,
                ["WorkerOfServiceCode"] = post.WorkerOfService.Code,

                ["PostCode"] = post.Code,
                ["PostTitle"] = post.Title,

                ["ServiceCode"] = post.Service.Code,
                ["ServiceName"] = post.Service.Name,

                ["FeedbackStatus"] = fb == null ? 0 : 1,
                ["PostStatus"] = post.status,
                ["ApplyStatus"] = apply.Status,
            });
            return new DynamicResult() { Message = "Success", Data = resp, Totalrow = 0, Type = "Success", Status = 1 };

        }
    }
}