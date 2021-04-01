using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using WorkAppReactAPI.Data.Interface;

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
        public ActionResult<DynamicResult> getAllUser()
        {
            var listuser = _repository.getAllUser();
            return Ok(listuser);
        }
        [HttpGet("{id}")]
        public ActionResult<DynamicResult> getUserById(Guid id)
        {
            var user = _repository.GetUserById(id); 
            return Ok(user);
        }
    }
}