using System;
using System.Threading.Tasks;
using WorkAppReactAPI.Configuration;
using WorkAppReactAPI.Dtos;
using WorkAppReactAPI.Dtos.Requests;

namespace WorkAppReactAPI.Data.Interface
{
    public interface IWorkerRepo
    {
        Task<DynamicResult> getWorker(Query model); 
        Task<DynamicResult> GetWorkerByPhone(String phone);
        Task<DynamicResult> AddWorker(UserUpdate model);
        Task<DynamicResult> UpdateWorker(string phone, UserUpdate model, UserLogin auth);
        Task<DynamicResult> DeleteWorker(string phone, UserLogin auth);
    }
    public class WorkerGet
    {
        public int Start { set; get; }
        public int Length { set; get; }
        public int Order { set; get; }

    }
}