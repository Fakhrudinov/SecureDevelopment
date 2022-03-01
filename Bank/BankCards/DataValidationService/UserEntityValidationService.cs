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
                    .WithErrorCode("U-101.1");

            RuleFor(x => x.Password)
                .NotEmpty()
                    .WithMessage("{PropertyName} не должен быть пустым")
                    .WithErrorCode("U-103.1")
                .Length(8, 30)
                    .WithMessage("{PropertyName} должен быть от 8 до 30 символов")
                    .WithErrorCode("U-104.1")
                .Matches("(^(?=[^А-Яа-я\\s])((?=.*[!@#$%^&*()\\-_=+{};:,<.>]){1})" +
                        "(?=[^А-Яа-я\\s]+\\d|\\d)((?=[^А-Яа-я\\s]+[a-z]|[a-z]){1})" +
                        "((?=[^А-Яа-я\\s]+[A-Z]|[A-Z]){1})[^А-Яа-я\\s]+$)")
                    .WithMessage("{PropertyName} должен содержать как минимум 1 Заглавную, 1 строчную, 1 цифру, 1 спецсимвол")
                    .WithErrorCode("U-105.1");
        }
    }
}
