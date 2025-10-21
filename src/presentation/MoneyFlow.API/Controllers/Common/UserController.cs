using Mediator.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyFlow.Application.DTOs.Common.Users;
using MoneyFlow.Application.UseCases.Common.Users.Commands.ChangePassword;
using MoneyFlow.Application.UseCases.Common.Users.Commands.Register;
using MoneyFlow.Application.UseCases.Common.Users.Commands.Update;
using MoneyFlow.Application.UseCases.Common.Users.Queries.GetLoggedUserProfile;
using MoneyFlow.Common.Communications;
using MoneyFlow.Domain.Common.Security;

namespace MoneyFlow.API.Controllers.Common;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("profile")]
    [ProducesResponseType(typeof(BaseResponse<GetUserFullQueryDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfile()
    { 
        var result = await _mediator.SendAsync(new GetLoggedUserProfileQuery());

        return Ok(result);
    }

    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] BaseRequest<UpdateUserProfileCommandDTO> request)
    {
        var command = new UpdateUserProfileCommand { user = request!.Data! };
        var result = await _mediator.SendAsync(command);
        return NoContent();
    }

    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] BaseRequest<UserChangePasswordCommand> request)
    {
        UserChangePasswordCommand command = request!.Data!;

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
