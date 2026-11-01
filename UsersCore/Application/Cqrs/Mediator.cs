using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Cqrs
{
    // Message IRequest<TResponse> Mensaje
    // Message IRequestHandler<TRequest, TResponse> Manejador
    // Message ISender<TResponse> Despachador
    // Message IPipelineBehvior Interceptores
    public static class Mediator
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            // Registro automatico de los contenedores y detecta IRequest y IRequestHandlers, arma diccionario
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            return services;
        }
    }
}
