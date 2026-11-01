using Application.Models.Dto;
using Domain.Contracts;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Handlers.Queries.User
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, ResponseModel<UserDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ResponseModel<UserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            if (request.userId == null)
            {
                return new ResponseModel<UserDto>
                {
                    Data = null!,
                    Type = ResponseModel<UserDto>.ResponseType.FAIL,
                    MessageKey = "UserIdNull",
                    Message = "User ID cannot be null.",
                    StatusCode = 400,
                    Success = false
                };
            }

            var result = await _userRepository.GetByIdAsync(request.userId, cancellationToken);

            if (result == null)
            {
                return new ResponseModel<UserDto>
                {
                    Data = null!,
                    Type = ResponseModel<UserDto>.ResponseType.FAIL,
                    MessageKey = "User Not Found",
                    Message = "User ID Not Found",
                    StatusCode = 404,
                    Success = false
                };
            }

            var user = mapUserToDto(result);

            var response = new ResponseModel<UserDto>
            {
                Data = user,
                Type = ResponseModel<UserDto>.ResponseType.SUCCESS,
                MessageKey = "UserFound",
                Message = "User found successfully.",
                StatusCode = 200,
                Success = true
            };

            return response;
        }

        private UserDto mapUserToDto(UserEntity user)
        {
            return new UserDto
            (
                id: user.Id,
                name: user.Name,
                age: user.Age,
                email: user.Email
            );
        }
    }
}
