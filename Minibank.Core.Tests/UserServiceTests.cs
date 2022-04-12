using Minibank.Core.Domains.Users;
using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Minibank.Core.Domains.Users.Services;
using Minibank.Core.Domains.Users.Repositories;
using System.Threading;

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
            var fakeUserId = "fakeId";

            _fakeUserRepository
                .Setup(repository => repository.GetById(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(new User() { Id = fakeUserId });

            var user = await _userService.GetById(fakeUserId, CancellationToken.None);

            Assert.Equal(user.Id, fakeUserId);
        }

        [Fact]
        public async Task GetUserById_WithNullId_ShouldThrowException()
        {
            var exception = await Assert.ThrowsAsync<Exception>(() => _userService.GetById(null, CancellationToken.None));

            Assert.Contains("Не задан id пользователя", exception.Message);
        }

        [Fact]
        public async Task AddUser_WithNullLogin_ShouldThrowException()
        {
            var user = new User { Login = null };

            var exception = await Assert.ThrowsAsync<Exception>(() =>_userService.Create(user, CancellationToken.None));

            Assert.Contains("Не задан логин пользователя", exception.Message);
        }
    }
}
