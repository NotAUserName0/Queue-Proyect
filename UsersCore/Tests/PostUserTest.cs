using Application.Handlers.Commands.User;
using Application.Models.Dto;
using Domain.Contracts;
using Domain.Entities;
using Infrastructure.Repositories;
using Moq;
using Tests.Config;

namespace Tests
{
    public class PostUserTest
    {
        public UserRepository GetRepository()
        {
            var db = DbContext.InitConnection();
            var context = DbContext.DataBaseContext(db);

            return new UserRepository(context);
        }

        [Fact]
        public async Task RegistrySuccessfullyAdded()
        {
            var repository = GetRepository();
            var user = new UserDto
            {
                Id = 1,
                Name = "Fermin Simulated 1",
                Age = 26,
                Email = "fjimenez@gmail.com",
                Password = "123331Ab$"
            };

            var command = new CreateUserCommand(user);
            var handler = new CreateUserCommandHandler(repository);
            var result = await handler.Handle(command, CancellationToken.None);

            // Validation
            Assert.NotNull(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task RegistryFailedAdding()
        {
            var repository = new Mock<IUserRepository>();
            var user = new UserDto
            {
                Id = 1,
                Name = "Fermin Simulated 1",
                Age = 26,
                Email = "fjimenez@gmail.com",
                Password = "123331Ab$"
            };

            repository.Setup(r => r.CreateAsync(It.IsAny<User>()))
                .ReturnsAsync(false);

            var handler = new CreateUserCommandHandler(repository.Object);
            var command = new CreateUserCommand(user);
            var result = await handler.Handle(command, CancellationToken.None);

            // Validation
            Assert.NotNull(result);
            Assert.False(result.Success);
        }
    }
}
