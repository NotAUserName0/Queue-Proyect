using System;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using QueueCore.Application.Interfaces;

namespace QueueCore.Functions;

public class UserQueueTrigger
{
    private readonly ILogger<UserQueueTrigger> _logger;
    private readonly IPetitionGenerator _petitionGenerator;

    public UserQueueTrigger(ILogger<UserQueueTrigger> logger, IPetitionGenerator petitionGenerator)
    {
        _logger = logger;
        _petitionGenerator = petitionGenerator;
    }

    [Function(nameof(UserQueueTrigger))]
    public async Task Run([QueueTrigger("solicitudes-pendientes", Connection = "AzureQueueConnection")] QueueMessage message)
    {
        _logger.LogInformation("Mensaje de la cola: {messageText}", message.MessageText);

        try
        {
            _logger.LogInformation("Procesando: {messageText}", message.MessageText);
            _logger.LogInformation("Waiting...");
            await Task.Delay(10000);
            _logger.LogInformation("Sending...");
            await _petitionGenerator.ProcessMessageAsync(message.MessageText);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al procesar el mensaje de la cola: {messageText}", message.MessageText);
        }
    }
}