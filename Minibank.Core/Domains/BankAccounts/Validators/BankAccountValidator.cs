using FluentValidation;
using Minibank.Core.Domains.BankAccounts.Enums;
using Minibank.Core.Domains.BankAccounts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.BankAccounts.Validators
{
    public class BankAccountValidator : AbstractValidator<BankAccount>
    {
        public BankAccountValidator(IBankAccountRepository bankAccountRepository)
        {
            RuleFor(x => x.UserId).NotEmpty()
                .WithMessage("Не должен быть пустым");
            RuleFor(x => x.Currency).Must(currency => !Enum.IsDefined(typeof(Currency), currency))
                .WithMessage("Запрещенна для использования");
        }
    }
}
