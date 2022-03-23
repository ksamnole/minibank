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
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public UserDto Get(string id)
        {
            var model = _userService.GetById(id);

            return new UserDto
            {
                Id = model.Id,
                Login = model.Login,
                Email = model.Email
            };
        }

        [HttpGet]
        public IEnumerable<UserDto> GetAll()
        {
            return _userService.GetAll()
                .Select(it => new UserDto
                {
                    Id = it.Id,
                    Login = it.Login,
                    Email = it.Email
                });
        }

        [HttpPost]
        public void Create(UserDto model)
        {
            _userService.Create(new User
            {
                Id = model.Id,
                Login = model.Login,
                Email = model.Email
            });
        }

        [HttpPut("{id}")]
        public void Update(string id, UserDto model)
        {
            _userService.Update(new User
            {
                Id = id,
                Login = model.Login,
                Email = model.Email
            });
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _userService.Delete(id);
        }
    }
}
