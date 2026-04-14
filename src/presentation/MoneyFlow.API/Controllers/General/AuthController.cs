using Microsoft.AspNetCore.Mvc;
using MoneyFlow.Application.DTOs.General.Auth;
using MoneyFlow.Application.UseCases.General.Auth.Commands.Login;
using SharedKernel.Communications;
using SharedKernel.Mediator;

namespace MoneyFlow.API.Controllers.General;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TokenDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponseError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] AuthLoginCommand authLoginCommand)
    {
        var result = await _mediator.SendAsync(authLoginCommand);

        if (result.IsSuccess)
            return Ok(result.Value);

        return BadRequest(new BaseResponseError(result.Errors!));
    }
}
