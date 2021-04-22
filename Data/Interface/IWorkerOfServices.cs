using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WorkAppReactAPI.Configuration;
using WorkAppReactAPI.Dtos;
using WorkAppReactAPI.Dtos.Requests;
 

namespace WorkAppReactAPI.Data.Interface
{
    public interface IWorkerOfServicesRepo
    {
        Task<DynamicResult> GetallWorkerOfServices(Query model);
        Task<DynamicResult> GetWorkerOfServicesDetail(Query model);
        Task<DynamicResult> RegisterWorkerOfServices(WorkerOfServicesUpdate model, UserLogin auth);  // insert
        Task<DynamicResult> VetificationWorkerOfServices(WorkerOfServicesUpdate model, UserLogin auth); // update
       
        Task<DynamicResult> DeleteWorkerOfServices(string WorkerOfServiceCode, UserLogin auth);
    }
    public class WorkerOfServicesRequest
    {
        public string Code { set; get; }
        public int Start { set; get; }
        public int Length { set; get; }
        public int Order { set; get; }

    }
    public class WorkerOfServicesUpdate
    {
        public string WorkerOfServicesCode { set; get; }
        public string ServiceCode { set; get; }
        public int isApproval { set; get; } // 0 : chưa duyệt, 1: duyệt thành công, 2 : duyệt thất bại
        public IFormFile Image { set; get; }

    }
}