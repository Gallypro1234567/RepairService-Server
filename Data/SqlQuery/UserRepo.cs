using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WorkAppReactAPI.Asset;
using WorkAppReactAPI.Assets;
using WorkAppReactAPI.Configuration;
using WorkAppReactAPI.Data.Interface;
using WorkAppReactAPI.Dtos.Requests;


namespace WorkAppReactAPI.Data.SqlQuery
{
    public class UserRepo : IUserRepo
    {


        private readonly WorkerServiceContext _context;
        private readonly IAuthorRepo _author;
        public UserRepo(WorkerServiceContext context,  IAuthorRepo author)
        {
            _context = context;
            _author = author;
        }

        public Task<DynamicResult> Login(UserLogin model)
        {
            throw new NotImplementedException();
        }

        public async Task<DynamicResult> Register(UserRegister model)
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
        public async Task<DynamicResult> ChangePassword(UserChangePassword model, UserLogin auth)
        {
            var result = new DynamicResult();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Phone == auth.Phone && x.Password == Encryptor.Encrypt(auth.Password));
            if (user == null)
            {

                return new DynamicResult() { Message = "Lỗi xác thực", Data = null, Totalrow = 0, Type = "Error-Validation", Status = 2 };

            }
            var DecryptPass = Encryptor.Decrypt(user.Password);
            if ( DecryptPass != model.OldPassword)
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

        public async Task<DynamicResult> UpdateInformation(UserUpdate model, UserLogin Auth)
        {
            var result = new DynamicResult();

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Phone == Auth.Phone && x.Password == Encryptor.Encrypt(Auth.Password));
            if (user == null)
            {

                return new DynamicResult() { Message = "Lỗi xác thực", Data = null, Totalrow = 0, Type = "Error-Validation", Status = 2 };

            }

            var checkCustomer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == user.Id);
            if (checkCustomer != null)
            {
                SqlParameter[] parameters1 = {
                    new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = user.Id },
                    new SqlParameter("@Phone", SqlDbType.VarChar) { Value = user.Phone},
                    new SqlParameter("@FullName", SqlDbType.NVarChar) { Value = model.Fullname},
                    new SqlParameter("@Birthday", SqlDbType.DateTime) { Value = model.Birthday},
                    new SqlParameter("@Email", SqlDbType.VarChar) { Value = model.Email},
                    new SqlParameter("@ImageUrl", SqlDbType.VarChar) { Value = model.ImageUrl},
                    new SqlParameter("@Address", SqlDbType.NVarChar) { Value = model.Address}
                };
                result = await _context.ExecuteDataTable("[dbo].[sp_UpdateCustomer]", parameters1).JsonDataAsync();
                return result;
            }

            SqlParameter[] parameters2 = {
                    new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = user.Id },
                    new SqlParameter("@Phone", SqlDbType.VarChar) { Value = model.Phone},
                    new SqlParameter("@FullName", SqlDbType.NVarChar) { Value = model.Fullname},
                    new SqlParameter("@Birthday", SqlDbType.DateTime) { Value = model.Birthday},
                    new SqlParameter("@Email", SqlDbType.VarChar) { Value = model.Email},
                    new SqlParameter("@ImageUrl", SqlDbType.VarChar) { Value = model.ImageUrl},
                    new SqlParameter("@Address", SqlDbType.NVarChar) { Value = model.Address},
                    new SqlParameter("@CMND", SqlDbType.VarChar) { Value = model.CMND},
                    new SqlParameter("@ImageUrlOfCMND", SqlDbType.VarChar) { Value = model.ImageUrlOfCMND}
                };
            result = await _context.ExecuteDataTable("[dbo].[sp_UpdateWorker]", parameters2).JsonDataAsync();
            return result;

        }


    }
}