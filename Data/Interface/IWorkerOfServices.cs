using System;
using WorkAppReactAPI.Configuration;

namespace WorkAppReactAPI.Data.Interface
{
    public interface IWorkerOfServices
    {
        DynamicResult GetallWorkerOfServices(); 
        DynamicResult RegisterWorkerOfServices(Guid ID, Guid WorkerID, Guid ServiceID, DateTime CreateAt); 
        DynamicResult VetificationWorkerOfServices(Guid ID,  bool isApproval);
        DynamicResult DeleteWorkerOfServices(Guid ID); 
    }
}