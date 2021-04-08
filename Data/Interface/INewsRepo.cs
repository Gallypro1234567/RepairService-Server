using System;
using WorkAppReactAPI.Configuration;


namespace WorkAppReactAPI.Data.Interface
{
    public interface INewsRepo
    {
        DynamicResult ListNews();
        DynamicResult AddNews(Guid ID, string Name, string Code, string description, string ImageUrl); 
        DynamicResult UpdateNews(Guid ID, string Name, string Code, string description, string ImageUrl); 
        DynamicResult DeleteNews(Guid ID); 
    }
}