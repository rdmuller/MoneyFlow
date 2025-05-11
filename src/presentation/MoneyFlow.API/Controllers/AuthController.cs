using Mediator.Abstractions;
using Microsoft.AspNetCore.Mvc;
using MoneyFlow.Application.UseCases.Auth.Commands.Login;

namespace MoneyFlow.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody]AuthLoginCommand authLoginCommand)
    {
        return Ok("");
    }
}
