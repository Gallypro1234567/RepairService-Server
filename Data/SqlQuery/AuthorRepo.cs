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
    public class AuthorRepo : IAuthorRepo
    {
        private readonly WorkerServiceContext _context;
        public AuthorRepo(WorkerServiceContext context)
        {
            _context = context;
        }


        public bool AuthorizeLogin(UserLogin auth)
        {

            var user = _context.Users.FirstOrDefault(x => x.Phone == auth.Phone && x.Password == Encryptor.Encrypt(auth.Password));
            if (user == null)
            {
                return false;
            }
            return true;
        }

        public async Task<dynamic> AuthorizeRole(UserLogin auth, string functionCode, int typeFuntion)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Phone == auth.Phone && x.Password == Encryptor.Encrypt(auth.Password));
            if (user == null)
            {

                return new DynamicResult() { Message = "Authorized Error", Data = null, Totalrow = 0, Type = "Error", Status = 2 };

            }
            var check = user.UserRoles.FirstOrDefault(x => x.Role.FunctionCode == functionCode);
            if (check == null)
            {
                return new DynamicResult() { Message = "Authorized is Denied", Data = null, Totalrow = 0, Type = "Error", Status = 2 };
            }
            switch (typeFuntion)
            {
                case 1:
                    {
                        if (check.Role.isSearch)
                        {

                            return true;
                        }
                        else
                        {
                            return new DynamicResult()
                            {
                                Message = "Authorized is Denied",
                                Type = "Error",
                                Status = 2
                            };
                        }
                    }
                case 2:
                    {
                        if (check.Role.isInsert)
                        {

                            return true;
                        }
                        else
                        {
                            return new DynamicResult()
                            {
                                Message = "Authorized is Denied",
                                Type = "Error",
                                Status = 2
                            };
                        }
                    }
                case 3:
                    {
                        if (check.Role.isUpdate)
                        {

                            return true;
                        }
                        else
                        {
                            return new DynamicResult()
                            {
                                Message = "Authorized is Denied",
                                Type = "Error",
                                Status = 2
                            };
                        }
                    }
                case 4:
                    {
                        if (check.Role.isDelete)
                        {

                            return true;
                        }
                        else
                        {
                            return new DynamicResult()
                            {
                                Message = "Authorized is Denied",
                                Type = "Error",
                                Status = 2
                            };
                        }
                    }
                case 5:
                    {
                        if (check.Role.isExport)
                        {

                            return true;
                        }
                        else
                        {
                            return new DynamicResult()
                            {
                                Message = "Authorized is Denied",
                                Type = "Error",
                                Status = 2
                            };
                        }
                    }
                case 6:
                    {
                        if (check.Role.isImport)
                        {

                            return true;
                        }
                        else
                        {
                            return new DynamicResult()
                            {
                                Message = "Authorized is Denied",
                                Type = "Error",
                                Status = 2
                            };
                        }
                    }
                default:
                    {
                        return new DynamicResult()
                        {
                            Message = "Authorized is Denied",
                            Type = "Error",
                            Status = 2
                        };
                    }
            }

        }


    }
}