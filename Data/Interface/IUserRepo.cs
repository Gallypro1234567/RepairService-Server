using System;
using System.Threading.Tasks;
using WorkAppReactAPI.Configuration;
using WorkAppReactAPI.Dtos.Requests;

namespace WorkAppReactAPI.Data.Interface
{
    public interface IUserRepo
    {

        Task<DynamicResult> Login(UserLogin model);
        Task<DynamicResult> Register(UserRegister model);
        Task<DynamicResult> ChangePassword(UserChangePassword model, UserLogin Auth);
        Task<DynamicResult> UpdateInformation(UserUpdate model, UserLogin Auth);

    }
}