using Architecture.Domain.Entities;
using FluentValidation;

namespace Architecture.Core.Services.Users
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.Id)
                .Empty()
                .WithMessage("Идентификатор может назначить только сама система!");

            RuleFor(x => x.Name)
                .Length(0, 255);

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Адрес почты не может быть пустым");

            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("Некорректный почтовый адрес");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Пароль недолжен быть пустым");
        }
    }
}
