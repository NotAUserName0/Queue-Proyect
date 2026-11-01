using Application.Handlers.Commands.Azure;
using Application.Models.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QueueController : ControllerBase
    {
        private readonly ISender _sender;

        public QueueController(ISender sender)
        {
            _sender = sender;
        }
        
        [HttpPost("send-to-queue")]
        public async Task<IActionResult> SendToQueue([FromBody] RequestDto request, CancellationToken cancellationToken)
        {
            try
            {

                var command = new AzureQueueCommand(request);
                var result = await _sender.Send(command, cancellationToken);

                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
