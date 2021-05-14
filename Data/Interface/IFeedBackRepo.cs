
using System.Threading.Tasks;
using WorkAppReactAPI.Configuration;
using WorkAppReactAPI.Dtos.Requests;

namespace WorkAppReactAPI.Data.Interface
{
    public interface IFeedBackRepo
    {
        Task<DynamicResult> getFeedBackByWofSCode(FeedbackQuery query);
        Task<DynamicResult> getWorkerRating(string wofscode);

        Task<DynamicResult> AddFeedBack(FeedbackUpdate model, UserLogin auth);
        Task<DynamicResult> DeleteFeedBack(string code, UserLogin auth);
    }
    public class FeedbackUpdate
    {
        public string workerofservicecode { set; get; }
        public string postcode { set; get; }
        public string description { set; get; }
        public double pointRating { set; get; }

    }
    public class FeedbackQuery
    {
        public string wofscode { set; get; }
        public int start { set; get; }
        public int length { set; get; }

    }
}