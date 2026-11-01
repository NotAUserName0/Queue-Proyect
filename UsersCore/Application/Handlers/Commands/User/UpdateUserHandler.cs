using Application.Models.Dto;
using Domain.Entities;
using Domain.Contracts;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Handlers.Commands.User
{
    public sealed class UpdateUserHandler : IRequestHandler<UpdateUserCommand, ResponseModel<bool>>
    {
        private readonly IUserRepository _userRepository;
        public UpdateUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<ResponseModel<bool>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var passwordHasher = new PasswordHasher<UserEntity>();
            var user = new UserEntity
            (
                id: request.user.Id,
                name: request.user.Name,
                age: request.user.Age,
                email: request.user.Email,
                password: request.user.Password
            );
            user.Password = passwordHasher.HashPassword(user, user.Password);

            var userExists = await _userRepository.GetByIdAsync(user.Id, cancellationToken);

            if (userExists == null)
            {
                return new ResponseModel<bool>
                {
                    Data = false,
                    Success = false,
                    Message = "User not found."
                };
            }

            var result = await _userRepository.UpdateAsync(user, cancellationToken);

            return new ResponseModel<bool>
            {
                Data = result,
                Success = result,
                Message = result ? "User updated successfully." : "Failed to update user."
            };
        }
    }
}
