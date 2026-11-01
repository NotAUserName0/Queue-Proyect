using Application.Models.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Handlers.Commands.User
{
    public sealed record UpdateUserCommand(UserDto user) : IRequest<ResponseModel<bool>>;
}
