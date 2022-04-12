using Minibank.Core.Domains.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Minibank.Core.Domains.Users.Services;
using Minibank.Core.Domains.Users.Repositories;
using System.Threading;
using FluentValidation;
using Minibank.Core.Domains.Users.Validators;

namespace Minibank.Core.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _fakeUserRepository;
        private readonly Mock<IUnitOfWork> _fakeUnitOfWork;
        private readonly IValidator<User> _userValidator;
        private readonly IUserService _userService;

        public UserServiceTests()
        {
            _fakeUserRepository = new Mock<IUserRepository>();
            _fakeUnitOfWork = new Mock<IUnitOfWork>();
            _userValidator = new UserValidator(_fakeUserRepository.Object);
            _userService = new UserService(_fakeUserRepository.Object, _userValidator ,_fakeUnitOfWork.Object);
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
        public async Task GetAllUsers_SuccessPath_ReturnListUsers()
        {
            await _userService.GetAll(CancellationToken.None);
            
            _fakeUserRepository.Verify(repository => repository.GetAll(CancellationToken.None));
        }

        [Fact]
        public async Task AddUser_SuccessPath()
        {
            var user = new User { Login = "test", Email = "test@test.ru"};

            await _userService.Create(user, CancellationToken.None);
            
            _fakeUserRepository.Verify(repository => repository.Create(user, CancellationToken.None));
        }
        
        [Fact]
        public async Task AddUser_WithNullLogin_ShouldThrowException()
        {
            var user = new User { Login = null, Email = "test"};

            var exception =  await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => _userService.Create(user, CancellationToken.None));
            var error = exception.Errors.First();
            
            Assert.Equal("Login", error.PropertyName);
            Assert.Equal("Не должен быть пустым", error.ErrorMessage);
        }
        
        [Fact]
        public async Task AddUser_WithEmptyLogin_ShouldThrowException()
        {
            var user = new User { Login = "", Email = "test@test.ru"};

            var exception =  await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => _userService.Create(user, CancellationToken.None));
            var error = exception.Errors.First();
            
            Assert.Equal("Login", error.PropertyName);
            Assert.Equal("Не должен быть пустым", error.ErrorMessage);
        }
        
        [Fact]
        public async Task DeleteUser_SuccessPath()
        {
            var fakeId = "fakeId";

            await _userService.Delete(fakeId, CancellationToken.None);
            
            _fakeUserRepository.Verify(repository => repository.Delete(fakeId, CancellationToken.None));
        }
        
        [Fact]
        public async Task UpdateUser_SuccessPath()
        {
            var user = new User { Id = "id6654654654", Login = "old", Email = "test@test" };

            // await _userService.Create(user, CancellationToken.None);
            //
            user.Login = "new";
            
            await _userService.Update(user, CancellationToken.None);
        }
    }
}
