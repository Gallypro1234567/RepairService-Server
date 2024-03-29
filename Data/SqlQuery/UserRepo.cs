using System;
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
    public class UserRepo : IUserRepo
    {

        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly WorkerServiceContext _context;

        public UserRepo(WorkerServiceContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _hostingEnvironment = webHostEnvironment;
        }
       

        public async Task<DynamicResult> Register(UserRegister model)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(x => x.Phone == model.Phone);
                if (user != null)
                {
                    return new DynamicResult() { Message = "Tài khoản này đã tồn tại", Data = null, Totalrow = 0, Type = "Error", Status = 2 };

                }

                Guid? key;
                do
                {
                    key = Guid.NewGuid();
                } while (_context.Users.FirstOrDefault(x => x.Id == key) != null);

                SqlParameter[] parameters ={
                    new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = Guid.NewGuid()},
                    new SqlParameter("@Phone", SqlDbType.VarChar) { Value = model.Phone},
                    new SqlParameter("@Password", SqlDbType.VarChar) { Value = model.Password},
                    new SqlParameter("@Fullname", SqlDbType.NVarChar) { Value = model.Fullname},
                    new SqlParameter("@Status", SqlDbType.NVarChar) { Value = 1},
                    new SqlParameter("@Role", SqlDbType.NVarChar) { Value = 1},
                };
                if (model.isCustomer)
                {
                    var result1 = await _context.ExecuteDataTable("[dbo].[sp_RegisterCustomer]", parameters).JsonDataAsync();

                    return result1;
                }
                else
                {
                    var result2 = await _context.ExecuteDataTable("[dbo].[sp_RegisterWorker]", parameters).JsonDataAsync();
                    return result2;
                }
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
        public async Task<DynamicResult> ChangePassword(UserChangePassword model, UserLogin auth)
        {
            var result = new DynamicResult();
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Phone == auth.Phone && x.Password == Encryptor.Encrypt(auth.Password));
                if (user == null)
                {

                    return new DynamicResult() { Message = "Lỗi xác thực", Data = null, Totalrow = 0, Type = "Error-Validation", Status = 2 };

                }
                var DecryptPass = Encryptor.Decrypt(user.Password);
                if (DecryptPass != model.OldPassword)
                {
                    return new DynamicResult() { Message = "Sai mật khẩu", Data = null, Totalrow = 0, Type = "Error", Status = 2 };

                }
                var newpassword = Encryptor.Encrypt(model.NewPassword);
                SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = user.Id },
                    new SqlParameter("@NewPassword", SqlDbType.VarChar) { Value = newpassword}
                };
                result = await _context.ExecuteDataTable("[dbo].[sp_ChangePassword]", parameters).JsonDataAsync();
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

        public async Task<DynamicResult> UpdateInformation(string phone, UserUpdate model, UserLogin Auth)
        {
            var result = new DynamicResult();
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Phone == Auth.Phone && x.Phone == phone && x.Password == Encryptor.Encrypt(Auth.Password));
                if (user == null)
                {

                    return new DynamicResult() { Message = "Account not found", Data = null, Totalrow = 0, Type = "Error-Validation", Status = 2 };

                }
                var ImageUrlDelete = user.ImageUrl;
                if (user.Phone != Auth.Phone && user.Password != Encryptor.Encrypt(Auth.Password))
                {
                    return new DynamicResult() { Message = "You can't modify  orther user's profile", Data = null, Totalrow = 0, Type = "Error-UnAuthorized", Status = 2 };
                }

                if (Auth.isCustomer)
                {
                    SqlParameter[] parameters1 = {
                    new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = user.Id },
                    new SqlParameter("@Phone", SqlDbType.VarChar) { Value = user.Phone},
                    new SqlParameter("@FullName", SqlDbType.NVarChar) { Value = model.Fullname},

                    new SqlParameter("@Sex", SqlDbType.Int) { Value = model.Sex},
                    new SqlParameter("@Email", SqlDbType.VarChar) { Value = model.Email == null ? DBNull.Value: model.Email },
                    new SqlParameter("@ImageUrl", SqlDbType.VarChar) { Value = model.ImageUrl == null ? DBNull.Value: model.ImageUrl },
                    new SqlParameter("@Address", SqlDbType.NVarChar) { Value = model.Address == null ? DBNull.Value: model.Address }
                };
                    result = await _context.ExecuteDataTable("[dbo].[sp_UpdateCustomer]", parameters1).JsonDataAsync();
                    return result;
                }

                SqlParameter[] parameters2 = {
                    new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = user.Id },
                    new SqlParameter("@Phone", SqlDbType.VarChar) { Value = user.Phone},
                    new SqlParameter("@FullName", SqlDbType.NVarChar) { Value = model.Fullname},
                    new SqlParameter("@Sex", SqlDbType.Int) { Value = model.Sex},
                    new SqlParameter("@Email", SqlDbType.VarChar) { Value = model.Email == null ? DBNull.Value: model.Email},
                    new SqlParameter("@ImageUrl", SqlDbType.VarChar) { Value = model.ImageUrl == null ? DBNull.Value: model.ImageUrl},
                    new SqlParameter("@Address", SqlDbType.NVarChar) { Value = model.Address == null ? DBNull.Value: model.Address},
                    new SqlParameter("@CMND", SqlDbType.VarChar) { Value = model.CMND == null ? DBNull.Value: model.CMND},
                    new SqlParameter("@ImageUrlOfCMND", SqlDbType.VarChar) { Value = model.ImageUrlOfCMND == null ?  DBNull.Value :  model.ImageUrlOfCMND}
                };
                result = await _context.ExecuteDataTable("[dbo].[sp_UpdateWorker]", parameters2).JsonDataAsync();
                if (result.Status == 1 && ImageUrlDelete != null)
                {
                    _hostingEnvironment.DeleteImage(ImageUrlDelete);
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

        public async Task<DynamicResult> Detail(UserLogin auth)
        {
            var result = new DynamicResult();
            SqlParameter[] parameters2 = {
                    new SqlParameter("@Phone", SqlDbType.VarChar) { Value = auth.Phone},

                };
            if (auth.isCustomer)
            {
                result = await _context.ExecuteDataTable("[dbo].[sp_GetCustomerbyPhone]", parameters2).JsonDataAsync();
            }
            else
            {
                result = await _context.ExecuteDataTable("[dbo].[sp_GetWorkerbyPhone]", parameters2).JsonDataAsync();
            }
            return result;

        }

        public async Task<DynamicResult> DisableAccount(string phone, int status, UserLogin Auth)
        {
            var result = new DynamicResult();
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Phone == phone);
                if (user == null)
                {
                    return new DynamicResult() { Message = "Không tìm thấy User", Data = null, Totalrow = 0, Type = "Error-Validation", Status = 2 };
                }
                var admin = await _context.Users.FirstOrDefaultAsync(x => x.Phone == Auth.Phone && x.Password == Encryptor.Encrypt(Auth.Password));
                if (admin == null)
                {
                    return new DynamicResult() { Message = "Không đủ quyền để xóa dữ liệu, liên hệ quản trị viên", Data = null, Totalrow = 0, Type = "Error-Validation", Status = 2 };
                }
                SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = user.Id },
                    new SqlParameter("@status", SqlDbType.Int) { Value = status },
                };
                result = await _context.ExecuteDataTable("[dbo].[sp_DisableAccount]", parameters).JsonDataAsync();
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