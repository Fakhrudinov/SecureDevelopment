using DataAbstraction;
using FluentValidation;

namespace DataValidationService
{
    public sealed class CardEntityValidationService : AbstractValidator<CardEntity>
    {
        public CardEntityValidationService()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                    .WithMessage("{PropertyName} '{PropertyValue}' должен быть больше нуля")
                    .WithErrorCode("M_100.1");

            RuleFor(x => x.HolderName)
                .Length(3, 30)
                    .WithMessage("{PropertyName} '{PropertyValue}' длинна должна быть от трех до 30 символов")
                    .WithErrorCode("M_101.1")
                .Matches("^((?:[A-Z]+ ?){1,3})$")
                    .WithMessage("{PropertyName} {PropertyValue} должно состоять только из заглавных символов латинского алфавита и не содержать цифры")
                    .WithErrorCode("M_101.2");

            RuleFor(x => x.Number)
                .Matches("^[0-9]{4} [0-9]{4} [0-9]{4} [0-9]{4}$")
                    .WithMessage("{PropertyName} '{PropertyValue}' не соответствует формату 'NNNN NNNN NNNN NNNN'")
                    .WithErrorCode("M_102.1");

            RuleFor(x => x.CVVCode)
                .Length(3, 3)
                    .WithMessage("{PropertyName} '{PropertyValue}' длинна должна быть 3 символа")
                    .WithErrorCode("M_103.1")
                .Matches("^[0-9]{3}$")
                    .WithMessage("{PropertyName} '{PropertyValue}' не соответствует формату 'NNN'")
                    .WithErrorCode("M_103.2");


            RuleFor(x => x.Type)
                .IsInEnum()
                    .WithMessage("{PropertyName} '{PropertyValue}' тип карты не удается распознать")
                    .WithErrorCode("M_104.1");
            RuleFor(x => x.System)
                .IsInEnum()
                    .WithMessage("{PropertyName} '{PropertyValue}' система карты не распознана")
                    .WithErrorCode("M_104.2");

            RuleFor(x => x.IsBlocked)
                .Must(x => x == false || x == true)
                    .WithMessage("{PropertyName} '{PropertyValue}' не является будевым значением")
                    .WithErrorCode("M_105.1");
        }
    }
}