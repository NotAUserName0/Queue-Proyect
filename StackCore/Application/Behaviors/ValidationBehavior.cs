using Application.Models.Dto;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Behaviors
{
    public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

            if (failures.Count != 0)
            {
                // Agrupamos los errores por nombre de propiedad (Key / Messages)
                var errorsDictionary = failures
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

                // Verificamos si la respuesta del handler espera un ResponseModel<>
                if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(ResponseModel<>))
                {
                    var innerType = typeof(TResponse).GetGenericArguments()[0];
                    var responseModelType = typeof(ResponseModel<>).MakeGenericType(innerType);

                    // Creamos la instancia usando tu constructor: (string Mensaje, bool Exitoso, T data)
                    var failedResponse = Activator.CreateInstance(
                        responseModelType,
                        "Se encontraron errores de validación",
                        false,
                        default!
                    );

                    // Asignamos el diccionario de errores a la nueva propiedad Errors
                    var errorsProperty = responseModelType.GetProperty("Errors");
                    errorsProperty?.SetValue(failedResponse, errorsDictionary);

                    return (TResponse)failedResponse!;
                }

                throw new ValidationException(failures);
            }

            return await next();
        }
    }
}
