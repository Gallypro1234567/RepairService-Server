using System;
using System.Threading.Tasks;
using WorkAppReactAPI.Configuration;
using WorkAppReactAPI.Dtos;
using WorkAppReactAPI.Dtos.Requests;

namespace WorkAppReactAPI.Data.Interface
{
    public interface ICustomerRepo
    {
        Task<DynamicResult> getCustomer(Query model);
        Task<DynamicResult> GetCustomerByPhone(String phone);
        Task<DynamicResult> AddCustomer(UserUpdate model);
        Task<DynamicResult> UpdateCustomer(string phone,UserUpdate model, UserLogin auth);
        Task<DynamicResult> DeleteCustomer(string phone, UserLogin auth);

    }
}