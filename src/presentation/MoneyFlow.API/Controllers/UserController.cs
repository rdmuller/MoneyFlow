using Mediator.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyFlow.Application.DTOs.Users;
using MoneyFlow.Application.UseCases.Users.Commands.ChangePassword;
using MoneyFlow.Application.UseCases.Users.Commands.Register;
using MoneyFlow.Application.UseCases.Users.Queries.GetLoggedUserProfile;
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
    [ProducesResponseType(typeof(BaseResponse<GetUserFullQueryDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfile()
    { 
        var result = await _mediator.SendAsync(new GetLoggedUserProfileQuery());

        return Ok(result);
    }

    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] BaseRequest<UserChangePasswordCommand> request)
    {
        UserChangePasswordCommand command = request.Data;

        var result = await _mediator.SendAsync(command);

        return Ok(result);
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
