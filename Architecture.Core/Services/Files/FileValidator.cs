using Architecture.Domain.Entities;
using FluentValidation;

namespace Architecture.Core.Services.Files
{
    public class FileValidator : AbstractValidator<File>
    {
        public FileValidator()
        {
            RuleFor(x => x.Id)
                .Empty()
                .WithMessage("Идентификатор может назначить только сама система!");

            RuleFor(x => x.FileName)
                .NotEmpty()
                .WithMessage("Имя файла не может быть пустым");

            RuleFor(x => x.Path)
                .NotEmpty()
                .WithMessage("Путь до файла не может быть пустым");
        }
    }
}
