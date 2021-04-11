using System;
using System.Threading.Tasks;
using WorkAppReactAPI.Configuration;
using WorkAppReactAPI.Dtos.Requests;


namespace WorkAppReactAPI.Data.Interface
{
    public interface IServiceRepo
    {
        Task<DynamicResult> getListService();

        Task<DynamicResult> AddService(ServiceUpdate model);

        Task<DynamicResult> UpdateService(ServiceUpdate model);

        Task<DynamicResult> DeleteService(ServiceDrop model);
    }
}