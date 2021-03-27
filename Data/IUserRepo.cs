using System;
using System.Collections.Generic;
using System.Data;
using WorkAppReactAPI.Models;

namespace WorkAppReactAPI.Data
{
    public interface IUserRepo
    {
        IEnumerable<User> getAllUser();
        User GetUserById(Guid id);
        DataTable GetUserByIds(Guid id);
    }
}