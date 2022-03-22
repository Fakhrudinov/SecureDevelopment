using DataAbstraction.AuthModels;
using FluentValidation;

namespace DataValidationService
{
    public class UserEntityValidationService : AbstractValidator<NewUser>
    {
        public UserEntityValidationService()
        {
            RuleFor(x => x.Login)
                .NotEmpty()
                    .WithMessage("{PropertyName} не должен быть пустым")
                    .WithErrorCode("U-100.1")
                .Length(4, 30)
                    .WithMessage("{PropertyName} должен быть от 4 до 30 символов")
                    .WithErrorCode("U-101.1")
                .Matches("^[a-zA-Z0-9_-]+$")
                    .WithMessage("{PropertyName} {PropertyValue} должен состоять только из символов латинского алфавита, цифр и символов '_-'")
                    .WithErrorCode("U_102.2");

            RuleFor(x => x.Password)
                .NotEmpty()
                    .WithMessage("{PropertyName} не должен быть пустым")
                    .WithErrorCode("U-103.1")
                .Length(8, 30)
                    .WithMessage("{PropertyName} {PropertyValue} должен быть от 8 до 30 символов")
                    .WithErrorCode("U-104.1")
                .Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[!@#$%^_-])[A-Za-z\\d!@#$%^_-]{8,}$")
                        //^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^_-])[A-Za-z\d!@#$%^_-]{8,}$
                    .WithMessage("{PropertyName} {PropertyValue} должен состоять только из символов латинского алфавита, цифр и символов '!@#$%^_-' " +
                    "и содержать как минимум 1 Заглавную, 1 строчную, 1 цифру, 1 спецсимвол")
                    .WithErrorCode("U-105.1");
        }
    }
}
