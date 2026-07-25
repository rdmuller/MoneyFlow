using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyFlow.Application.UseCases.General.Users.Commands.Register;
using Shared.Application.Messaging;
using Shared.Domain;
using SharedKernel.Communications;

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
            return BadRequest(result.Errors);

        return Created("", BaseResponse<string>.CreateNewObjectIdResponse(result.Value));
    }
}
