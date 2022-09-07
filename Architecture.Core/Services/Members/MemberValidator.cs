using Architecture.Domain.Entities;
using FluentValidation;

namespace Architecture.Core.Services.Members
{
    public class MemberValidator : AbstractValidator<Member>
    {
        public MemberValidator()
        {
            RuleFor(x => x.Id).Empty();
            RuleFor(x => x.Name).NotEmpty().WithMessage("Не заполнено имя (((( ");
            RuleFor(x => x.YouTubeUserId).NotEmpty().WithMessage("С таким YouTubeId клиент уже существует");
        }
    }
}
