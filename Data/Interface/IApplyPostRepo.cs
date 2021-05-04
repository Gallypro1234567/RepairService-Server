
using System.Threading.Tasks;
using WorkAppReactAPI.Configuration;
using WorkAppReactAPI.Dtos.Requests;

namespace WorkAppReactAPI.Data.Interface
{
    public interface IApplyToPostRepo
    {
        Task<DynamicResult> getApplytoPostbyCode(string code);
        Task<DynamicResult> getApplytoPostbyWorkerPhone(string phone);
        Task<DynamicResult> AddApplytoPost(ApplyToPostUpdate model, UserLogin auth);
        Task<DynamicResult> UpdateApplytoPost(ApplyToPostUpdate model, UserLogin auth);
        Task<DynamicResult> DeleteApplytoPost(ApplyToPostUpdate model, UserLogin auth);
    }
    public class ApplyToPostUpdate
    {
        public int status { set; get; }
        public string workerofservicecode { set; get; }
        public string postcode { set; get; }
    }
}