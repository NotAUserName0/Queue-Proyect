using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Application.Handlers.Commands.System.Validator
{
    public sealed class SystemCommandValidator : AbstractValidator<SystemCommand>
    {
        public SystemCommandValidator()
        {
            RuleFor(x => x.Message)
                .NotEmpty()
                .MinimumLength(3);
        }

    }
}
