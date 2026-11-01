using Application.Interfaces;
using Application.Models.Dto;
using Azure.Storage.Queues;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Services
{
    public class AzureQueueService : IAzureQueueService
    {
        private readonly QueueClient _queueClient;

        public AzureQueueService(IOptions<AzureConfigurationDto> configuration)
        {
            //Se obtiene el conection string y el nombre de la cola desde la configuración
            var connectionString = configuration.Value.ConnectionString
                ?? throw new InvalidOperationException("AzureQueue:ConnectionString no configurado.");
            var queueName = configuration.Value.QueueName
                ?? throw new InvalidOperationException("AzureQueue:QueueName no configurado.");

            var options = new QueueClientOptions(QueueClientOptions.ServiceVersion.V2024_08_04)
            {
                MessageEncoding = QueueMessageEncoding.Base64
            };

            // Inicializamos el cliente de la cola de Azure
            _queueClient = new QueueClient(connectionString, queueName, options);
        }

        public async Task SendMessageAsync(string messageContent, CancellationToken cancellationToken = default)
        {
            // Nos aseguramos de que la cola exista en Azure antes de enviar
            await _queueClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

            // Enviamos el mensaje (el SDK de Azure se encarga de codificarlo en Base64 automáticamente)
            await _queueClient.SendMessageAsync(messageContent, cancellationToken);
        }
    }
}
