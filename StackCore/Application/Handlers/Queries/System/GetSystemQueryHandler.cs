using Application.Models.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Handlers.Queries.System
{
    public sealed class GetSystemQueryHandler : IRequestHandler<GetSystemQuery, StatusDto>
    {
        public Task<StatusDto> Handle(GetSystemQuery request, CancellationToken cancellationToken)
        {
            var response = new StatusDto(
                Status:"Handler",
                Message: "Prueba",
                Timestamp: DateTime.UtcNow
                );

            return Task.FromResult(response);
        }
    }
}
