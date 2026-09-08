using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyFlow.Application.DTOs.General.Users;
using MoneyFlow.Application.UseCases.General.Users.Commands.Register;
using MoneyFlow.Application.UseCases.General.Users.Queries.GetLoggedUserProfile;
using Shared.Application.Messaging;
using Shared.Domain;
using Shared.Presentation.Communications;

namespace MoneyFlow.Presentation.Controllers.General;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsersController : ControllerBase
{
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Register(
        [FromServices] ICommandHandler<RegisterUserCommand, string> handler, 
        [FromBody] BaseRequest<RegisterUserCommand> command)
    {
        Result<string> result = await handler.HandleAsync(command.Data);

        if (result.IsFailure)
            return BadRequest(BaseResponse<string>.CreateFailureResponse(result.Errors!));

        return Created("", BaseResponse<string>.CreateNewObjectIdResponse(result.Value));
    }

    [HttpGet("profile")]
    [ProducesResponseType(typeof(BaseResponse<GetUserFullQueryDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetProfile(
        [FromServices] IQueryHandler<GetLoggedUserProfileQuery, GetUserFullQueryDTO> handler)
    {
        Result<GetUserFullQueryDTO> result = await handler.HandleAsync(new GetLoggedUserProfileQuery());

        return result.IsSuccess ? Ok(BaseResponse<GetUserFullQueryDTO>.CreateSuccessResponse(result.Value)) : NoContent();
    }
}
