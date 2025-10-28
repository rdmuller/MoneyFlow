using Mediator.Abstractions;
using Microsoft.AspNetCore.Mvc;
using MoneyFlow.Application.UseCases.General.Auth.Commands.Login;

namespace MoneyFlow.API.Controllers.Common;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody]AuthLoginCommand authLoginCommand)
    {
        var result = await _mediator.SendAsync(authLoginCommand);
        return Ok(result);
    }
}
