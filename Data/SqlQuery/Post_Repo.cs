using System;
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
    public class PostRepo : IPostRepo
    {
        private readonly WorkerServiceContext _context;
        public PostRepo(WorkerServiceContext context)
        {
            _context = context;
        }
        public async Task<DynamicResult> GetPost(PostGet model)
        {
            SqlParameter[] parameters ={
                new SqlParameter("@Code", SqlDbType.VarChar) { Value = model.ServiceCode},
                new SqlParameter("@start", SqlDbType.Int) { Value = model.Start},
                new SqlParameter("@lenght", SqlDbType.Int) { Value = model.Lenght},
                new SqlParameter("@order", SqlDbType.Int) { Value = model.Order},
                new SqlParameter("@status", SqlDbType.Int) { Value = model.Status},
            };
            var result = await _context.ExecuteDataTable("[dbo].[sp_GetAllPosts]", parameters).JsonDataAsync();
            return result;
        }


        public async Task<DynamicResult> GetPostByCustomer(PostGet model, UserLogin auth)
        {
            var result = new DynamicResult();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Phone == auth.Phone);
            if (user == null)
            {
                return new DynamicResult() { Message = "Account already Exists", Data = null, Totalrow = 0, Type = "Error", Status = 2 };

            }

            SqlParameter[] parameters ={
                new SqlParameter("@CustomerPhone", SqlDbType.VarChar) { Value = auth.Phone},
                new SqlParameter("@start", SqlDbType.Int) { Value = model.Start},
                new SqlParameter("@lenght", SqlDbType.Int) { Value = model.Lenght},
                new SqlParameter("@order", SqlDbType.Int) { Value = model.Order},
                new SqlParameter("@status", SqlDbType.Int) { Value = model.Status},
            };
            var Customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == user.Id);

            if (Customer == null)
            {
                return new DynamicResult() { Message = "Account already Exists", Data = null, Totalrow = 0, Type = "Error", Status = 2 };
            }
            result = await _context.ExecuteDataTable("[dbo].[sp_GetAllPostsByCustomer]", parameters).JsonDataAsync();
            return result;
        }

        public async Task<DynamicResult> GetPostByWorker(PostGet model, UserLogin auth)
        {
            var result = new DynamicResult();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Phone == auth.Phone);
            if (user == null)
            {
                return new DynamicResult() { Message = "Account already Exists", Data = null, Totalrow = 0, Type = "Error", Status = 2 };

            }
            SqlParameter[] parameters ={
                new SqlParameter("@WorkerPhone", SqlDbType.VarChar) { Value = auth.Phone},
                new SqlParameter("@start", SqlDbType.Int) { Value = model.Start},
                new SqlParameter("@lenght", SqlDbType.Int) { Value = model.Lenght},
                new SqlParameter("@order", SqlDbType.Int) { Value = model.Order},
                new SqlParameter("@status", SqlDbType.Int) { Value = model.Status},
            };
            var worker = await _context.Workers.FirstOrDefaultAsync(x => x.Id == user.Id);

            if (worker == null)
            {
                return new DynamicResult() { Message = "Account already Exists", Data = null, Totalrow = 0, Type = "Error", Status = 2 };
            }
            result = await _context.ExecuteDataTable("[dbo].[sp_GetAllPostsByWorker]", parameters).JsonDataAsync();
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
                //
                new SqlParameter("@Title", SqlDbType.NVarChar) { Value = model.Title},
                new SqlParameter("@Position", SqlDbType.VarChar) { Value = model.Position == null ? DBNull.Value: model.Position},
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

            var post = await _context.Posts.FirstOrDefaultAsync(x => x.Code == code);
            if (post == null)
            {
                return new DynamicResult() { Message = "Post not found", Data = null, Totalrow = 0, Type = "Error", Status = 2 };
            }

            // 
            SqlParameter[] parameters ={
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = post.Id},
                new SqlParameter("@Code", SqlDbType.VarChar) { Value = post.Code},
                new SqlParameter("@CreateAt", SqlDbType.DateTime) { Value = post.CreateAt},
                //
                new SqlParameter("@CustomerID", SqlDbType.UniqueIdentifier) { Value = post.Customer.Id},
                new SqlParameter("@WorkerOfServiceID", SqlDbType.UniqueIdentifier) { Value = DBNull.Value},
                //
                new SqlParameter("@Title", SqlDbType.NVarChar) { Value = model.Title},
                new SqlParameter("@Position", SqlDbType.VarChar) { Value = model.Position == null ? DBNull.Value: model.Position},
                new SqlParameter("@Address", SqlDbType.NVarChar) { Value = model.Address  == null ? DBNull.Value: model.Address},
                new SqlParameter("@ImageUrl", SqlDbType.VarChar) { Value = model.ImageUrl == null ? DBNull.Value: model.ImageUrl},
                new SqlParameter("@FinishAt", SqlDbType.DateTime) { Value = model.FinishAt},
                //
                new SqlParameter("@status", SqlDbType.Int) { Value = model.status},
            };

            result = await _context.ExecuteDataTable("[dbo].[sp_UpdatePost]", parameters).JsonDataAsync();
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
                var check = await _context.WorkerOfServices.Include(x =>x.Worker).FirstOrDefaultAsync(x => x.Worker.Id == user.Id);
                if (check == null)
                {
                    return new DynamicResult() { Message = "Account already Exists", Data = null, Totalrow = 0, Type = "Error", Status = 2 };
                }

                var post = await _context.Posts.Include(x =>x.Customer).FirstOrDefaultAsync(x => x.Code == code);
                if (post == null)
                {
                    return new DynamicResult() { Message = "Post not found", Data = null, Totalrow = 0, Type = "Error", Status = 2 };
                }

                // 
                SqlParameter[] parameters ={
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = post.Id},
                new SqlParameter("@Code", SqlDbType.VarChar) { Value = post.Code},
                new SqlParameter("@CreateAt", SqlDbType.DateTime) { Value = post.CreateAt},
                //
                new SqlParameter("@CustomerID", SqlDbType.UniqueIdentifier) { Value = post.Customer.Id},
                //
                new SqlParameter("@WorkerOfServiceID", SqlDbType.UniqueIdentifier) { Value = check.Id},
                //
                new SqlParameter("@Title", SqlDbType.NVarChar) { Value = post.Title},
                new SqlParameter("@Position", SqlDbType.VarChar) { Value = post.Position == null ? DBNull.Value : post.Position},
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
            if (checkPost == null)
            {
                return new DynamicResult() { Message = "You can't not delete post of orther user", Data = null, Totalrow = 0, Type = "Error", Status = 2 };
            }
            if (checkPost.status > 0)
            {
                return new DynamicResult() { Message = "You can't not delete post when it is processing", Data = null, Totalrow = 0, Type = "Error", Status = 2 };
            }

            SqlParameter[] parameters ={
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = checkPost.Id}
            };
            var result = await _context.ExecuteDataTable("[dbo].[sp_DeletePost]", parameters).JsonDataAsync();
            return result;
        }

        // public async Task<DynamicResult> GetPostByUserId(UserLogin auth)
        // {
        //     var result = new DynamicResult();
        //     var user = await _context.Users.FirstOrDefaultAsync(x => x.Phone == auth.Phone);
        //     if (user == null)
        //     {
        //         return new DynamicResult() { Message = "Account already Exists", Data = null, Totalrow = 0, Type = "Error", Status = 2 };

        //     }
        //     SqlParameter[] parameters ={
        //         new SqlParameter("@UserID", SqlDbType.UniqueIdentifier) { Value = user.Id},
        //     };
        //     var isCustomer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == user.Id);
        //     if (isCustomer != null)
        //     {
        //         result = await _context.ExecuteDataTable("[dbo].[sp_GetPostByWorkerId]", parameters).JsonDataAsync();

        //     }
        //     else
        //     {
        //         result = await _context.ExecuteDataTable("[dbo].[sp_GetPostByWorkerId]", parameters).JsonDataAsync();

        //     }

        //     return result;
        // }
    }
}