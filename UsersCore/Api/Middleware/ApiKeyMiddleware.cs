using Api.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace Api.Middleware
{
    public sealed class ApiKeyMiddleware
    {
        private const string ApiKeyHeaderName = "ApiKey";
        private readonly RequestDelegate _next;
        private readonly SecurityConfiguration _securitySettings;

        public ApiKeyMiddleware(RequestDelegate next, IOptions<SecurityConfiguration> securitySettings)
        {
            _next = next;
            _securitySettings = securitySettings.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // 1. Validar si el header existe
            if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
            {
                await WriteUnauthorizedResponseAsync(context, "El header 'ApiKey' no fue proporcionado.");
                return;
            }

            // 2. Comparar de forma segura (previene ataques de timing)
            var expectedBytes = Encoding.UTF8.GetBytes(_securitySettings.ApiKey);
            var providedBytes = Encoding.UTF8.GetBytes(extractedApiKey.ToString());

            if (!CryptographicOperations.FixedTimeEquals(expectedBytes, providedBytes))
            {
                await WriteUnauthorizedResponseAsync(context, "El 'ApiKey' proporcionado es inválido.");
                return;
            }

            // 3. Si todo está bien, dejamos pasar la petición
            await _next(context);
        }

        // Respuesta asyncrona de no autorizacion
        private static async Task WriteUnauthorizedResponseAsync(HttpContext context, string detail)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/problem+json";

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "No Autorizado",
                Detail = detail,
                Instance = context.Request.Path
            };

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
