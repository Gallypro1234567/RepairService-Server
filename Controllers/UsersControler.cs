using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WorkAppReactAPI.Data; 
using WorkAppReactAPI.Models;

namespace WorkAppReactAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepo _repository;

        public UsersController(IUserRepo repository)
        {
            _repository = repository;
    
        }
        [HttpGet]
        public ActionResult<IEnumerable<User>> getAllUser()
        {
            var listuser = _repository.getAllUser();
            return Ok(listuser);
        }
        [HttpGet("{id}")]
        public ActionResult<User> getUserById(Guid id)
        {
            var user = _repository.GetUserById(id);
         
            return Ok(user);
        }
    }
}