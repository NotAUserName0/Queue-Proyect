using Domain.Contracts;
using Application.Models.Dto;
using MediatR;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Handlers.Commands.User
{
    public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ResponseModel<bool>>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ResponseModel<bool>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var userEntered = request.user;
            var passwordHasher = new PasswordHasher<UserEntity>();

            var user = new UserEntity
            (
                name: userEntered.Name,
                age: userEntered.Age,
                email: userEntered.Email, // unique control de error en db y a lvl code
                password: userEntered.Password
            );

            string hashedPassword = passwordHasher.HashPassword(user, user.Password);
            user.Password = hashedPassword;

            var result = await _userRepository.CreateAsync(user, cancellationToken).ConfigureAwait(false);

            return new ResponseModel<bool>("Created User", true, result);
        }
    }
}
