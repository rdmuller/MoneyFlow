using Mediator.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyFlow.Application.DTOs.Users;
using MoneyFlow.Application.UseCases.Users.Commands.Register;
using MoneyFlow.Common.Communications;
using MoneyFlow.Domain.Security;

namespace MoneyFlow.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController(IMediator mediator, ITokenProvider tokenProvider) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly ITokenProvider _tokenProvider = tokenProvider;

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    { 
        return Ok(_tokenProvider.TokenOnRequest());
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterUser([FromBody] BaseRequest<RegisterUserCommandDTO> request)
    {
        var command = new RegisterUserCommand{ User = request.Data };
        var result = await _mediator.SendAsync(command);
        return Created("", result);
    }
}
