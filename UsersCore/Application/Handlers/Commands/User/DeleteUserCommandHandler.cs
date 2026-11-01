using Application.Models.Dto;
using Domain.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Handlers.Commands.User
{
    public sealed class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, ResponseModel<bool>>
    {
        private readonly IUserRepository _userRepository;
        public DeleteUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<ResponseModel<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var userExists = await _userRepository.GetByIdAsync(request.userId, cancellationToken);
            if (userExists == null)
            {
                return new ResponseModel<bool>
                {
                    Data = false,
                    Success = false,
                    Message = "User not found."
                };
            }
            var result = await _userRepository.DeleteAsync(userExists, cancellationToken);
            return new ResponseModel<bool>
            {
                Data = result,
                Success = result,
                Message = result ? "User deleted successfully." : "Failed to delete user."
            };
        }
    }
}
