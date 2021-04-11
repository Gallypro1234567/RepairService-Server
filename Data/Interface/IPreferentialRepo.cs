using System;
using System.Threading.Tasks;
using WorkAppReactAPI.Configuration;
using WorkAppReactAPI.Dtos.Requests;

namespace WorkAppReactAPI.Data.Interface
{
    public interface IPreferentialRepo
    {
        Task<DynamicResult> ListPreferentials();
        Task<DynamicResult> AddPreferential(PreferentialUpdate model);
        Task<DynamicResult> UpdatePreferential(PreferentialUpdate model);
        Task<DynamicResult> DeletePreferential(PreferentialDrop model);
    }
}