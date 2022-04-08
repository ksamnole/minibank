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
        public async Task<UserDto> Get(string id)
        {
            var model = await _userService.GetById(id);

            return new UserDto
            {
                Id = model.Id,
                Login = model.Login,
                Email = model.Email
            };
        }

        [HttpGet]
        public async Task<IEnumerable<UserDto>> GetAll()
        {
            var users = await _userService.GetAll();

            return users.Select(it => new UserDto
                {
                    Id = it.Id,
                    Login = it.Login,
                    Email = it.Email
                });
        }

        [HttpPost]
        public async Task Create(CreateUserDto model)
        {
            await _userService.Create(new User
            {
                Login = model.Login,
                Email = model.Email
            });
        }

        [HttpPut("{id}")]
        public async Task Update(string id, CreateUserDto model)
        {
            await _userService.Update(new User
            {
                Id = id,
                Login = model.Login,
                Email = model.Email
            });
        }

        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _userService.Delete(id);
        }
    }
}
