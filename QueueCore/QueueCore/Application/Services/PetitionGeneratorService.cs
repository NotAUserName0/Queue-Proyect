using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QueueCore.Application.Interfaces;
using QueueCore.Application.Models.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace QueueCore.Application.Services
{
    public class PetitionGeneration : IPetitionGenerator
    {
        private readonly IApiConsumer _apiConsumer;
        private readonly ILogger<PetitionGeneration> _logger;
        private readonly IOptions<ClientDataDto> _configuration;

        public PetitionGeneration(IApiConsumer apiConsumer, ILogger<PetitionGeneration> logger, IOptions<ClientDataDto> configuration)
        {
            _apiConsumer = apiConsumer;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task ProcessMessageAsync(string messageText)
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var queueMessage = JsonSerializer.Deserialize<UserQueueDto>(messageText, options);

            if (queueMessage == null || string.IsNullOrEmpty(queueMessage.Action))
            {
                _logger.LogWarning("Mensaje inválido o carece de una 'Action'.");
                return;
            }

            var baseUrl = _configuration.Value.BaseUrl;
            var jsonPayload = JsonSerializer.Serialize(queueMessage.UserData);
            bool success = false;

            // Función local para extraer el userId de forma segura del JSON
            string ObtenerUserId()
            {
                var jsonNode = JsonNode.Parse(jsonPayload);
                if (jsonNode == null) return string.Empty;

                // Busca las tres variantes posibles de forma directa
                var idNode = jsonNode["Id"] ?? jsonNode["id"] ?? jsonNode["userId"];

                return idNode?.ToString() ?? string.Empty;
            }

            var request = new ApiRequestDto();

            // Construcción de rutas exactas basadas en tu Swagger
            switch (queueMessage.Action.ToUpper())
            {
                case "CREATE":
                    request = new ApiRequestDto(
                        method: HttpMethod.Post,
                        endpoint: $"{baseUrl}/api/User/createUser",
                        payload: jsonPayload
                        );
                    success = await _apiConsumer.SendRequestAsync(request);
                    break;
                case "UPDATE":
                    request = new ApiRequestDto(
                        method: HttpMethod.Put,
                        endpoint: $"{baseUrl}/api/User/updateUser",
                        payload: jsonPayload
                        );
                    success = await _apiConsumer.SendRequestAsync(request);
                    break;
                case "DELETE":
                    request = new ApiRequestDto(
                        method: HttpMethod.Delete,
                        endpoint: $"{baseUrl}/api/User/deleteUser/{ObtenerUserId()}"
                        );
                    success = await _apiConsumer.SendRequestAsync(request);
                    break;
                // Estos metodos son de prueba para redirigir el trafico a otra url
                case "GETUSER":
                    request = new ApiRequestDto(
                        method: HttpMethod.Get,
                        endpoint: $"{baseUrl}/api/User/getUser/{ObtenerUserId()}"
                        );
                    // Se envía null en el payload porque un GET no lleva body
                    success = await _apiConsumer.SendRequestAsync(request);
                    break;
                case "GETUSERS":
                    request = new ApiRequestDto(
                        method: HttpMethod.Get,
                        endpoint: $"{baseUrl}/api/User/getUsers"
                        );
                    success = await _apiConsumer.SendRequestAsync(request);
                    break;
                default:
                    throw new NotSupportedException($"Acción '{queueMessage.Action}' no soportada.");
            }

            if (!success)
            {
                throw new Exception($"Falló la petición hacia el API 2 para la acción {queueMessage.Action}");
            }

            _logger.LogInformation($"La acción {queueMessage.Action} se procesó y envió correctamente.");
        }
    }
 }
