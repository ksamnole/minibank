using Minibank.Core.Domains.Users.Repositories;
using System.Collections.Generic;

namespace Minibank.Core.Domains.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User GetById(string id)
        {
            return _userRepository.GetById(id);
        }

        public IEnumerable<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        public void Create(User user)
        {
            _userRepository.Create(user);
        }

        public void Delete(string id)
        {
            _userRepository.Delete(id);
        }

        public void Update(User user)
        {
            _userRepository.Update(user);
        }
    }
}
