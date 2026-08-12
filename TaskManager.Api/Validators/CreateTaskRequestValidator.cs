using FluentValidation;
using TaskManager.Api.Contracts;
using TaskManager.Consts;

namespace TaskManager.Api.Validators;

public class CreateTaskRequestValidator : AbstractValidator<CreateTaskRequest>
{
    public CreateTaskRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Название не может быть пустым");

        RuleFor(x => x.Title)
            .MaximumLength(MainConsts.TASK_TITLE_SIZE)
            .WithMessage($"Название должно содержать до {MainConsts.TASK_TITLE_SIZE} символов");

        RuleFor(x => x.Priority)
            .IsInEnum()
            .WithMessage("Указан некорректный приоритет");
    }
}