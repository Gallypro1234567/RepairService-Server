using System;
using System.Collections.Generic;
using System.Data;
using WorkAppReactAPI.Models;

namespace WorkAppReactAPI.Data
{
    public class MockUserRepo : IUserRepo
    {
      
        public IEnumerable<User> getAllUser()
        {
          var  list = new List<User>{
              new User{id= Guid.NewGuid(), name = "Trần Văn A", address = "Cần thơ 1"},
              new User{id= Guid.NewGuid(), name = "Trần Văn B", address = "Cần thơ 2"},
              new User{id= Guid.NewGuid(), name = "Trần Văn C", address = "Cần thơ 3"},
              new User{id= Guid.NewGuid(), name = "Trần Văn D", address = "Cần thơ 4"},
          };
          return list;
        }

        public User GetUserById(Guid id)
        {
            return new User{id= Guid.NewGuid(), name = "Trần Văn A", address = "Cần thơ"};
        }

        public DataTable GetUserByIds(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}