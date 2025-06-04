using Mediator.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyFlow.Application.DTOs.Users;
using MoneyFlow.Application.UseCases.Users.Commands.Register;
using MoneyFlow.Common.Communications;

namespace MoneyFlow.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetProfile()
    { 
        return Ok("");
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterUser([FromBody] BaseRequest<RegisterUserCommandDTO> request)
    {
        var command = new RegisterUserCommand{ User = request.Data };
        var result = await _mediator.SendAsync(command);
        return Created("", result);
    }
}
