using FluentValidation;
using Minibank.Core.Domains.Users.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.Users.Validators
{
    public class UserValidator : AbstractValidator<User>
    { 
        public UserValidator(IUserRepository userRepository)
        {
            RuleFor(x => x.Login).NotEmpty()
                .WithMessage("Не должен быть пустым");
            RuleFor(x => x.Login.Length).LessThanOrEqualTo(20)
                .WithName("Длина логина")
                .WithMessage("Не должен превышать 20 символов");
            RuleFor(x => x.Login).Must(login => !userRepository.ContainsByLogin(login))
                .WithName("Логин")
                .WithMessage("Уже используется");

            RuleFor(x => x.Email).NotEmpty()
                .WithMessage("Не должен быть пустым");
        }
    }
}
