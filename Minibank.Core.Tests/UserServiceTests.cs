using Minibank.Core.Domains.Users;
using Minibank.Data.Users.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Minibank.Core.Domains.Users.Services;
using Minibank.Core.Domains.Users.Repositories;
using Minibank.Data.Users;

namespace Minibank.Core.Tests
{
    public class UserServiceTests
    {
        private readonly IUserService _userService;
        private readonly Mock<IUserRepository> _fakeUserRepository;
        private readonly Mock<IUnitOfWork> _fakeUnitOfWork;

        public UserServiceTests()
        {
            _fakeUserRepository = new Mock<IUserRepository>();
            _fakeUnitOfWork = new Mock<IUnitOfWork>();
            _userService = new UserService(_fakeUserRepository.Object, _fakeUnitOfWork.Object);
        }

        [Fact]
        public async Task GetUserById_SuccessPath_ReturnUserModel()
        {
            // ARRANGE
            _fakeUserRepository
                .Setup(repository => repository.GetById(It.IsAny<string>()))
                .Returns(new User());

            // ACT
            var exception = await Assert.ThrowsAsync<Exception>(() => _userService.Create(user));

            // ASSERT
            Assert.Contains("Не задан логин пользователя", exception.Message);
        }

        [Fact]
        public async Task AddUser_WithNullLogin_ShouldThrowException()
        {
            // ARRANGE
            var user = new User { Login = null };

            // ACT
            var exception = await Assert.ThrowsAsync<Exception>(() =>_userService.Create(user));

            // ASSERT
            Assert.Contains("Не задан логин пользователя", exception.Message);
        }

        //[Fact]
        //public async Task AddUser_DuplicatedLogin_ShouldThrowException()
        //{
        //    // ARRANGE
        //    _fakeUserRepository
        //        .Setup(repository => repository.C(It.IsAny<string>()))
        //        .Returns(true);
        //    var user = new User { Login = "Login" };

        //    // ACT
        //    var exception = await Assert.ThrowsAsync<Exception>(() => _userService.Create(user));

        //    // ASSERT
        //    Assert.Contains("Не задан логин пользователя", exception.Message);
        //}

        //[Fact]
        //public async Task GetUserById_SuccessPath_ReturnUserModel()
        //{
        //    // ARRANGE
        //    var user = new User { Login = null };

        //    // ACT
        //    var exception = await Assert.ThrowsAsync<Exception>(() => _userService.Create(user));

        //    // ASSERT
        //    Assert.Contains("Не задан логин пользователя", exception.Message);
        //}
    }
}
