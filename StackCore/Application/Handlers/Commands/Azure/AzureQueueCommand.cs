using Application.Models.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Handlers.Commands.Azure
{
    public sealed record AzureQueueCommand(RequestDto Request) : IRequest<ResponseModel<bool>>;
}
