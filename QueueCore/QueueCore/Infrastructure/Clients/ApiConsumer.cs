using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using QueueCore.Application.Interfaces;
using QueueCore.Application.Models.Dto;
using QueueCore.Functions;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueueCore.Infrastructure.Clients
{
    //Metodo para armar la peticion HTTP y enviarla al endpoint correspondiente
    public class ApiConsumer : IApiConsumer
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<UserQueueTrigger> _logger;
        private readonly ILoggerFile _loggerFile;
        private LogDto log;

        public ApiConsumer(HttpClient httpClient, ILogger<UserQueueTrigger> logger, ILoggerFile loggerFile)
        {
            _httpClient = httpClient;
            _logger = logger;
            _loggerFile = loggerFile;
        }

        public async Task<bool> SendRequestAsync(HttpMethod method, string endpoint, string payload = null)
        {
            var request = new HttpRequestMessage(method, endpoint);
            _logger.LogInformation("Request: {message}", request.ToString());

            if (payload != null)
            {
                request.Content = new StringContent(payload, Encoding.UTF8, "application/json");
            }

            log = new LogDto(
                method: method,
                endpoint:  endpoint,
                payload: payload
            );
            await _loggerFile.GenerateLog(log);

            var response = await _httpClient.SendAsync(request);
            // Leemos el cuerpo real de la respuesta de forma asíncrona
            var responseBody = await response.Content.ReadAsStringAsync();

            // Imprimimos el string real en el log
            _logger.LogInformation("Response: {message}", responseBody);

            log = new LogDto
            (
                method: method,
                endpoint: endpoint,
                payload: payload,
                statusCode: (int)response.StatusCode,
                responseBody: responseBody
            );
            await _loggerFile.GenerateLog(log);

            return response.IsSuccessStatusCode;
        }
    }
}
