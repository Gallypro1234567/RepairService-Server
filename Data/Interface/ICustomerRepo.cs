using System;
using System.Threading.Tasks;
using WorkAppReactAPI.Configuration;
using WorkAppReactAPI.Dtos.Requests;

namespace WorkAppReactAPI.Data.Interface
{
    public interface ICustomerRepo
    {
        Task<DynamicResult> getAllCustomer();
        Task<DynamicResult> GetCustomerByCode(String phone);
        Task<DynamicResult> AddCustomer(UserUpdate model);
        Task<DynamicResult> UpdateCustomer(UserUpdate model);
        Task<DynamicResult> DeleteCustomerById(UserUpdate model);

    }
}