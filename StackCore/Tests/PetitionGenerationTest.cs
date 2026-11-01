using Application.Handlers.Commands.Azure;
using Application.Interfaces;
using Moq;

namespace Tests
{
    // Asegura que cumple con la responsabilidad de emitir la orden de encolar un mensaje
    public class PetitionGenerationTest
    {
        [Fact]
        public async Task PetitionsOnQueue()
        {
            //Para servicios no bd se simula en memoria la interfaz de la cola para no tocar la parte de application
            var mockQueueService = new Mock<IAzureQueueService>();

            // Simula una respuesta real del servicio enviando datos cualquiera con el tipo de dato requerido
            mockQueueService
                .Setup(q => q.SendMessageAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask); // regresa respuesta correcta

            // Injectamos el servicio duplicado al handler
            var handler = new AzureQueueCommandHandler(mockQueueService.Object);

            // generamos un command
            var queue = new Application.Models.Dto.RequestDto
            {
                Action = "create",
                UserData = new Application.Models.Dto.UserDto
                {
                    Id = 1004,
                    Name = "logger test z",
                    Age = 26,
                    Email = "fjgi@live.com",
                    Password = "12345$aBqw"
                }
            };
            var command = new AzureQueueCommand(queue);

            // ejecutamos command en el handler
            await handler.Handle(command, CancellationToken.None);

            //verificamos la respuesta
            mockQueueService.Verify(q => q.SendMessageAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }

        [Fact]
        public async Task PetitionFailedQueue()
        {
            //Para servicios no bd se simula en memoria la interfaz de la cola para no tocar la parte de application
            var mockQueueService = new Mock<IAzureQueueService>();

            // Simula una respuesta real del servicio enviando datos cualquiera con el tipo de dato requerido
            mockQueueService
                .Setup(q => q.SendMessageAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new System.Exception("Error simulado de conexion")); // simula error de conexion

            // Injectamos el servicio duplicado al handler
            var handler = new AzureQueueCommandHandler(mockQueueService.Object);

            // generamos un command
            var queue = new Application.Models.Dto.RequestDto
            {
                Action = "create",
                UserData = new Application.Models.Dto.UserDto
                {
                    Id = 1004,
                    Name = "logger test z",
                    Age = 26,
                    Email = "fjgi@live.com",
                    Password = "12345$aBqw"
                }
            };
            var command = new AzureQueueCommand(queue);

            // ejecutamos command en el handler
            await handler.Handle(command, CancellationToken.None);

            //verificamos la respuesta enviando una solicitud any
            mockQueueService.Verify(q => q.SendMessageAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }
    }
}
