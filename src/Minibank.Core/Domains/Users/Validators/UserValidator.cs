using FluentValidation;
using Minibank.Core.Domains.Users.Repositories;

namespace Minibank.Core.Domains.Users.Validators
{
    public class UserValidator : AbstractValidator<User>
    { 
        public UserValidator(IUserRepository userRepository)
        {
            RuleFor(x => x.Login).NotEmpty()
                .WithMessage("Не должен быть пустым");
            RuleFor(x => x.Login).Must(login => !userRepository.ContainsByLogin(login))
                .WithMessage("Уже используется");

            RuleFor(x => x.Email).NotEmpty()
                .WithMessage("Не должен быть пустым");
        }
    }
}
