using System;
using System.Threading.Tasks;
using WorkAppReactAPI.Configuration;
using WorkAppReactAPI.Dtos;
using WorkAppReactAPI.Dtos.Requests;

namespace WorkAppReactAPI.Data.Interface
{
    public interface IPreferentialRepo
    {
        Task<DynamicResult> ListPreferentials(Query model);
        Task<DynamicResult> AddPreferential(PreferentialUpdate model, UserLogin auth);
        Task<DynamicResult> UpdatePreferential(string code,PreferentialUpdate model, UserLogin auth);
        Task<DynamicResult> DeletePreferential(string code, UserLogin auth);
    }
}