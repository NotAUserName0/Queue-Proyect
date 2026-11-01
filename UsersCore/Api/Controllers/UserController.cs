using Application.Handlers.Commands.User;
using Application.Handlers.Queries.User;
using Application.Interfaces;
using Application.Models.Dto;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using UsersCore.Application.Models.Dto;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class UserController : ControllerBase
    {
        private const int PageSizeDefault = 50;
        private const int PageSizeMax = 200;

        private readonly ISender _sender;
        private readonly IFileLogger _fileLogger;

        public UserController(ISender sender, IFileLogger fileLogger)
        {
            _sender = sender;
            _fileLogger = fileLogger;
        }

        [HttpPost("createUser")]
        [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser([FromBody] UserDto request, CancellationToken cancellationToken)
        {
            try
            {
                var command = new CreateUserCommand(request);
                var result = await _sender.Send(command, cancellationToken);

                if (result.Success)
                {
                    return Ok(result);
                }

                var log = new LogDto
                {
                    LogType = "command",
                    ResponseBody = JsonSerializer.Serialize(result.Data),
                    Message = JsonSerializer.Serialize(result.Errors)
                };
                await _fileLogger.GenerateLog(log);
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                var log = new LogDto
                {
                    ResponseBody = JsonSerializer.Serialize(ex.Data)
                };
                await _fileLogger.GenerateLog(log);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<object>
                {
                    Success = false,
                    Message = ex.Message,
                    StatusCode = 500
                });
            }
        }

        [HttpGet("getUser/{userId}")]
        [ProducesResponseType(typeof(ResponseModel<UserDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUser([FromRoute] int userId, CancellationToken cancellationToken)
        {
            try
            {
                var query = new GetUserQuery(userId);
                var result = await _sender.Send(query, cancellationToken);

                if (result.Success)
                {
                    return Ok(result);
                }

                return Problem(
                    detail: result.Message,
                    statusCode: result.StatusCode,
                    title: "Ocurrió un error al procesar la solicitud"
                );
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: ex.Message,
                    statusCode: 500,
                    title: "Ocurrió un error al procesar la solicitud"
                );
            }
        }

        [HttpGet("getUsers")]
        [ProducesResponseType(typeof(ResponseModel<UserDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllUsers([FromQuery] int page = 1, [FromQuery] int pageSize = PageSizeDefault, CancellationToken cancellationToken = default)
        {
            try
            {
                if (pageSize > PageSizeMax) pageSize = PageSizeMax;
                if (pageSize < 1) pageSize = PageSizeDefault;
                if (page < 1) page = 1;

                var query = new GetUsersQuery(page, pageSize);
                var result = await _sender.Send(query, cancellationToken);

                if (result.Success)
                {
                    return Ok(result);
                }

                return Problem(
                    detail: result.Message,
                    statusCode: result.StatusCode,
                    title: "Ocurrió un error al procesar la solicitud"
                );
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: ex.Message,
                    statusCode: 500,
                    title: "Ocurrió un error al procesar la solicitud"
                );
            }
        }

        [HttpPut("updateUser")]
        [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateUser([FromBody] UserDto request, CancellationToken cancellationToken)
        {
            try
            {
                var command = new UpdateUserCommand(request);
                var result = await _sender.Send(command, cancellationToken);

                if (result.Success)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<object>
                {
                    Success = false,
                    Message = ex.Message,
                    StatusCode = 500
                });
            }
        }

        [HttpDelete("deleteUser/{userId}")]
        [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteUser([FromRoute] int userId, CancellationToken cancellationToken)
        {
            try
            {
                var command = new DeleteUserCommand(userId);
                var result = await _sender.Send(command, cancellationToken);

                if (result.Success)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<object>
                {
                    Success = false,
                    Message = ex.Message,
                    StatusCode = 500
                });
            }
        }
    }
}
