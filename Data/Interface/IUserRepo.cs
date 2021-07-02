using System;
using System.Threading.Tasks;
using WorkAppReactAPI.Configuration;
using WorkAppReactAPI.Dtos.Requests;

namespace WorkAppReactAPI.Data.Interface
{
    public interface IUserRepo
    {

        Task<DynamicResult> Register(UserRegister model);
        Task<DynamicResult> Detail(UserLogin auth); 
        Task<DynamicResult> ChangePassword(UserChangePassword model, UserLogin Auth);
        Task<DynamicResult> UpdateInformation(string phone, UserUpdate model, UserLogin Auth);
        Task<DynamicResult> DisableAccount(string phone, int status, UserLogin Auth);

    }
}