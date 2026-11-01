using Application.Handlers.Commands.System;
using Application.Handlers.Queries.System;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class SystemController : ControllerBase
    {
        private readonly ISender _sender;

        public SystemController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("liveness")]
        public async Task<IActionResult> GetLiveness(CancellationToken cancellationToken)
        {
            var query = new GetSystemQuery();
            var result = await _sender.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpPost("ping")]
        public async Task<IActionResult> SendPing([FromBody] string Message, CancellationToken cancellationToken)
        {
            var command = new SystemCommand(Message);
            var result = await _sender.Send(command, cancellationToken);

            return Ok(result);
        }
    }
}
