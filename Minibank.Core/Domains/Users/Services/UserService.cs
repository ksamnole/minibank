using Minibank.Core.Domains.Users.Repositories;
using System.Collections.Generic;

namespace Minibank.Core.Domains.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public User Get(string id)
        {
            return userRepository.Get(id);
        }

        public IEnumerable<User> GetAll()
        {
            return userRepository.GetAll();
        }

        public void Create(User user)
        {
            userRepository.Create(user);
        }

        public void Delete(string id)
        {
            userRepository.Delete(id);
        }

        public void Update(User user)
        {
            userRepository.Update(user);
        }
    }
}
