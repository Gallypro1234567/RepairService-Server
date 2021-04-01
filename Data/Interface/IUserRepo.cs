using System;
using WorkAppReactAPI.Controllers;

namespace WorkAppReactAPI.Data.Interface
{
    public interface IUserRepo
    {
        DynamicResult getAllUser();
        DynamicResult GetUserById(Guid id);
       
    }
}