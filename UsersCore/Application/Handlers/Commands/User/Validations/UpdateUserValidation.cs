using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Handlers.Commands.User.Validations
{
    public sealed class UpdateUserValidation : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserValidation()
        {
            RuleFor(x => x.user.Id)
                .GreaterThan(0).WithMessage("El Id del usuario debe ser mayor que 0.");
            RuleFor(x => x.user.Name)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no debe exceder los 100 caracteres.");
            RuleFor(x => x.user.Age)
                .InclusiveBetween(1, 120).WithMessage("La edad debe estar entre 1 y 120.");
            RuleFor(x => x.user.Email)
                .NotEmpty().WithMessage("El correo electrónico es obligatorio.")
                .EmailAddress().WithMessage("Formato de correo electrónico inválido.");
            RuleFor(x => x.user.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.")
                .Matches(@"[A-Z]").WithMessage("La contraseña debe contener al menos una letra mayúscula.")
                .Matches(@"[a-z]").WithMessage("La contraseña debe contener al menos una letra minúscula.")
                .Matches(@"[0-9]").WithMessage("La contraseña debe contener al menos un número.")
                .Matches(@"[\W_]").WithMessage("La contraseña debe contener al menos un carácter especial (ej. @, $, !, %, *, ?, &).");
        }
    }
}
