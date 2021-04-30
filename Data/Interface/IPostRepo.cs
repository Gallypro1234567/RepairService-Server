using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WorkAppReactAPI.Configuration;
using WorkAppReactAPI.Dtos.Requests;

namespace WorkAppReactAPI.Data.Interface
{
    public interface IPostRepo
    {
        Task<DynamicResult> GetPost(PostGet model);
        Task<DynamicResult> GetPostDetail(string Code);
        Task<DynamicResult> GetRecentlyPosts(PostGet model);
        Task<DynamicResult> GetPostByPhone(string phone, PostGet model);
        Task<DynamicResult> InserPost(PostUpdate model, UserLogin auth);
        Task<DynamicResult> UpdatePostByCustomer(string code, PostUpdate model, UserLogin auth);
        Task<DynamicResult> UpdatePostByWorker(string code, UserLogin auth);
        Task<DynamicResult> DeletePost(string postCode, UserLogin auth);

    }
    public class PostGet
    {
        public string ServiceCode { set; get; }
        public string WofSCode { set; get; }
        public int Start { set; get; }
        public int Length { set; get; }
        public int Order { set; get; }
        public int? Status { set; get; }

    }
    public class PostUpdate
    {
        public string Code { set; get; }
        public string ServiceCode { set; get; }
        public string Title { set; get; }
        public string Position { set; get; }
        public string Address { set; get; }
        public string ImageUrl { set; get; }
        public List<IFormFile> Image { set; get; }
        public DateTime CreateAt { set; get; }
        public DateTime FinishAt { set; get; }
        public int status { set; get; }  // 0: khởi tạo, 1: đang thực hiện, 2 đã hoàn thành, 3 hủy , 4: xóa

    }
}