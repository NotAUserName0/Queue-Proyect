using Application.Models.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Handlers.Commands.System
{
    public sealed class SystemCommandHandler : IRequestHandler<SystemCommand, StatusDto>
    {
        public Task<StatusDto> Handle(SystemCommand request, CancellationToken cancellationToken)
        {
            var response = new StatusDto(
                Status: "POST",
                Message: request.Message,
                Timestamp: DateTime.UtcNow
                );

            return Task.FromResult(response);
        }
    }
}
