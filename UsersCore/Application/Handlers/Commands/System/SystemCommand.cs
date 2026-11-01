using Application.Models.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Handlers.Commands.System
{
    public sealed record SystemCommand(string Message) : IRequest<StatusDto>;
}
