using Microsoft.AspNetCore.Mvc;
using Minibank.Core.Domains.Users;
using Minibank.Core.Domains.Users.Services;
using Minibank.Web.Controllers.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minibank.Web.Controllers.Users
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("{id}")]
        public UserDto Get(string id)
        {
            var model = userService.Get(id);

            return new UserDto
            {
                Id = model.Id,
                Login = model.Login,
                Email = model.Email
            };
        }

        [HttpGet()]
        public IEnumerable<UserDto> GetAll()
        {
            return userService.GetAll()
                .Select(it => new UserDto
                {
                    Id = it.Id,
                    Login = it.Login,
                    Email = it.Email
                });
        }


        [HttpPost("create")]
        public void Create(UserDto model)
        {
            userService.Create(new User
            {
                Id = model.Id,
                Login = model.Login,
                Email = model.Email
            });
        }

        [HttpPut("update/{id}")]
        public void Update(string id, UserDto model)
        {
            userService.Update(new User
            {
                Id = id,
                Login = model.Login,
                Email = model.Email
            });
        }

        [HttpDelete("delete/{id}")]
        public void Delete(string id)
        {
            userService.Delete(id);
        }
    }
}
