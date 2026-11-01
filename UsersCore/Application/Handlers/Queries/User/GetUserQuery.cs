using Application.Models.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Handlers.Queries.User
{
    public sealed record GetUserQuery(int userId) : IRequest<ResponseModel<UserDto>>;
}
