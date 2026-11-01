using Application.Interfaces;
using Application.Models.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Application.Handlers.Commands.Azure
{
    public class AzureQueueCommandHandler : IRequestHandler<AzureQueueCommand, ResponseModel<bool>>
    {
        private readonly IAzureQueueService _azureQueueService;

        public AzureQueueCommandHandler(IAzureQueueService azureQueueService)
        {
            _azureQueueService = azureQueueService;
        }

        public async Task<ResponseModel<bool>> Handle(AzureQueueCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var user = command.Request.UserData;
                var payload = new
                {
                    command.Request.Action,
                    UserData = new UserDto
                    (
                        id: user.Id,
                        name: user.Name,
                        age: user.Age,
                        email: user.Email,
                        password: user.Password
                    ),
                    DateTime.UtcNow
                };

                var jsonPayload = JsonSerializer.Serialize(payload);
                await _azureQueueService.SendMessageAsync(jsonPayload, cancellationToken);

                return new ResponseModel<bool>
                {
                    Data = true,
                    Type = ResponseModel<bool>.ResponseType.SUCCESS,
                    MessageKey = "AzureQueueCommandHandler.Success",
                    Message = "Message sent to Azure Queue successfully.",
                    StatusCode = 200,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<bool>
                {
                    Data = false,
                    Type = ResponseModel<bool>.ResponseType.FAIL,
                    MessageKey = "AzureQueueCommandHandler.Failure",
                    Message = $"Failed to send message to Azure Queue: {ex.Message}",
                    StatusCode = 500,
                    Success = false
                };
            }
        }
    }
}
