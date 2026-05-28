using Microsoft.AspNetCore.Mvc;
using MoneyFlow.Application.DTOs.General.Auth;
using MoneyFlow.Application.UseCases.General.Auth.Commands.Login;
using Shared.Application.Messaging;
using Shared.Domain;

namespace MoneyFlow.Presentation.Controllers.General;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TokenDTO), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(BaseResponseError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] AuthLoginCommand authLoginCommand)
    {
        Result<TokenDTO> result = await _mediator.SendAsync(authLoginCommand);

        if (result.IsSuccess)
            return Ok(result.Value);

        return BadRequest(/*new BaseResponseError(result.Errors!)*/ result.Errors);
    }
}
