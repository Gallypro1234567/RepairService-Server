using System;
using System.Threading.Tasks;
using WorkAppReactAPI.Configuration;
using WorkAppReactAPI.Dtos.Requests;


namespace WorkAppReactAPI.Data.Interface
{
    public interface IServiceRepo
    {
        Task<DynamicResult> getListService();

        Task<DynamicResult> AddService(ServicePost model);

        Task<DynamicResult> UpdateService(ServicePost model);

        Task<DynamicResult> DeleteService(ServicePost model);
    }
}