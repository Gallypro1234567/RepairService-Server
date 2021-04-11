using System;
using System.Threading.Tasks;
using WorkAppReactAPI.Configuration;
using WorkAppReactAPI.Dtos.Requests;

namespace WorkAppReactAPI.Data.Interface
{
    public interface IAuthorRepo
    {
        bool AuthorizeLogin(UserLogin auth);
        Task<dynamic> AuthorizeRole(UserLogin auth, string functionCode, int typeFuntion);
        

    }
}