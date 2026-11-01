using Application.Models.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Handlers.Queries.System
{
    // Indica a MadiatR que systemQuery regresa un dto
    public sealed record GetSystemQuery() : IRequest<StatusDto>;
}
