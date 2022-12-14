using Minibank.Core.Domains.Users.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;
using FluentValidation;

namespace Minibank.Core.Domains.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<User> _userValidator;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUserRepository userRepository, IValidator<User> userValidator, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _userValidator = userValidator;
            _unitOfWork = unitOfWork;
        }

        public async Task<User> GetById(string id, CancellationToken cancellationToken)
        {
            return await _userRepository.GetById(id, cancellationToken);
        }

        public async Task<IEnumerable<User>> GetAll(CancellationToken cancellationToken)
        {
            return await _userRepository.GetAll(cancellationToken);
        }

        public async Task Create(User user, CancellationToken cancellationToken)
        {
            await _userValidator.ValidateAndThrowAsync(user, cancellationToken);

            await _userRepository.Create(user, cancellationToken);
            await _unitOfWork.SaveChange();
        }

        public async Task Delete(string id, CancellationToken cancellationToken)
        {
            await _userRepository.Delete(id, cancellationToken);
            await _unitOfWork.SaveChange();
        }

        public async Task Update(User user, CancellationToken cancellationToken)
        {
            await _userValidator.ValidateAndThrowAsync(user, cancellationToken);

            await _userRepository.Update(user, cancellationToken);
            await _unitOfWork.SaveChange();
        }
    }
}
