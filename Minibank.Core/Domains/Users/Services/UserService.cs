using Minibank.Core.Domains.Users.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<User> GetById(string id)
        {
            return await _userRepository.GetById(id);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _userRepository.GetAll();
        }

        public async Task Create(User user)
        {
            await _userRepository.Create(user);
            await _unitOfWork.SaveChange();
        }

        public async Task Delete(string id)
        {
            await _userRepository.Delete(id);
            await _unitOfWork.SaveChange();
        }

        public async Task Update(User user)
        {
            await _userRepository.Update(user);
            await _unitOfWork.SaveChange();
        }
    }
}
