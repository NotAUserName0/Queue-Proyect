using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Handlers.Commands.User.Validations
{
    public sealed class CreateUserValidation : AbstractValidator<CreateUserCommand>
    {
        public CreateUserValidation()
        {
            RuleFor(x => x.user).ChildRules(userValidation =>
            {
                userValidation.RuleFor(u => u.Name)
                    .NotEmpty().WithMessage("El nombre es obligatorio")
                    .MaximumLength(50);
                userValidation.RuleFor(u => u.Email)
                    .NotEmpty().WithMessage("El correo es obligatorio")
                    .MaximumLength(100);
                userValidation.RuleFor(u => u.Password)
                    .NotEmpty().WithMessage("La contraseña es obligatoria.")
                    .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.")
                    .Matches(@"[A-Z]").WithMessage("La contraseña debe contener al menos una letra mayúscula.")
                    .Matches(@"[a-z]").WithMessage("La contraseña debe contener al menos una letra minúscula.")
                    .Matches(@"[0-9]").WithMessage("La contraseña debe contener al menos un número.")
                    .Matches(@"[\W_]").WithMessage("La contraseña debe contener al menos un carácter especial (ej. @, $, !, %, *, ?, &).");
                userValidation.RuleFor(u => u.Age)
                    .NotNull().WithMessage("El edad es obligatorio")
                    .GreaterThan(0).WithMessage("La edad debe ser mayor que 0")
                    .LessThanOrEqualTo(120).WithMessage("La edad debe ser menor o igual a 120");

            });
        }
    }
}
