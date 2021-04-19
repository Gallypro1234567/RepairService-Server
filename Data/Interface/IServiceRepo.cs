using System;
using System.Threading.Tasks;
using WorkAppReactAPI.Configuration;
using WorkAppReactAPI.Dtos;
using WorkAppReactAPI.Dtos.Requests;


namespace WorkAppReactAPI.Data.Interface
{
    public interface IServiceRepo
    {
        Task<DynamicResult> getListService(Query model);

        Task<DynamicResult> AddService(ServiceUpdate model,UserLogin auth);

        Task<DynamicResult> UpdateService(string code,ServiceUpdate model,UserLogin auth);

        Task<DynamicResult> DeleteService(string code, UserLogin auth);
    }
}