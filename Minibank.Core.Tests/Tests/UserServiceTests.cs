using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Minibank.Core.Domains.Users;
using Minibank.Core.Domains.Users.Repositories;
using Minibank.Core.Domains.Users.Services;
using Minibank.Core.Domains.Users.Validators;
using Moq;
using Xunit;

namespace Minibank.Core.Tests.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly IValidator<User> _userValidator;
        private readonly IUserService _userService;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _userValidator = new UserValidator(_mockUserRepository.Object);
            _userService = new UserService(_mockUserRepository.Object, _userValidator ,_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetUserById_SuccessPath_ReturnUserModel()
        {
            var fakeId = "id";
            
            _mockUserRepository
                .Setup(repository => repository.GetById(fakeId, CancellationToken.None))
                .ReturnsAsync(new User() { Id = fakeId });

            var user = await _userService.GetById(fakeId, CancellationToken.None);

            Assert.Equal(typeof(User), user.GetType());
        }

        [Fact]
        public async Task GetAllUsers_SuccessPath_ReturnListUsers()
        {
            var fakeUsers = new List<User>()
            {
                new User() { Id = "1" },
                new User() { Id = "2" },
            };
            
            _mockUserRepository
                .Setup(repository => repository.GetAll(CancellationToken.None))
                .ReturnsAsync(fakeUsers);
            
            var users = await _userService.GetAll(CancellationToken.None);

            _mockUserRepository.Verify(repository => repository.GetAll(CancellationToken.None));
            
            Assert.Equal(fakeUsers, users);
        }

        [Fact]
        public async Task AddUser_SuccessPath_CreateCalledOneTime()
        {
            var user = new User { Login = "test", Email = "test@test.ru"};

            await _userService.Create(user, CancellationToken.None);

            _mockUnitOfWork.Verify(verify => verify.SaveChange());
            _mockUserRepository.Verify(repository => repository.Create(user, CancellationToken.None));
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
        public async Task DeleteUser_SuccessPath_DeleteCalledOneTime()
        {
            await _userService.Delete("fakeId", CancellationToken.None);
            
            _mockUserRepository.Verify(repository => repository.Delete("fakeId", CancellationToken.None));
        }
        
        [Fact]
        public async Task UpdateUser_SuccessPath_UpdateCalledOneTime()
        {
            var user = new User { Id = "Id", Login = "Login", Email = "test@test" };

            await _userService.Update(user, CancellationToken.None);
            
            _mockUserRepository.Verify(repository => repository.Update(user, CancellationToken.None));
        }
        
        [Fact]
        public async Task UpdateUser_WithNullLogin_ShouldThrowException()
        {
            var user = new User { Id = "Id", Login = null, Email = "test@test" };
            
            var exception =  await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => _userService.Update(user, CancellationToken.None));
            var error = exception.Errors.First();

            Assert.Equal("Login", error.PropertyName);
            Assert.Equal("Не должен быть пустым", error.ErrorMessage);
        }
    }
}
