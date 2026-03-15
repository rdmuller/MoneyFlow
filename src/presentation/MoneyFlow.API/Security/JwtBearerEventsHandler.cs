using Mediator;
using Mediator.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MoneyFlow.Application.UseCases.General.Users.Queries.GetByExternalId;
using System.Security.Claims;

namespace MoneyFlow.API.Security;

internal class JwtBearerEventsHandler : JwtBearerEvents
{
    public override async Task TokenValidated(TokenValidatedContext context)
    {
        var userExternalIdClaim = context.Principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)!.Value;
        if (string.IsNullOrWhiteSpace(userExternalIdClaim))
        {
            context.Fail("Invalid token");
            return;
        }

        if (!Guid.TryParse(userExternalIdClaim, out var userExternalId))
        {
            context.Fail("Invalid token");
            return;
        }

        var mediator = context.HttpContext.RequestServices.GetRequiredService<IMediator>();
        var userDTO = await mediator.SendAsync(new GetUserByExternalIdQuery(userExternalId));
        if (userDTO is null || userDTO.Data is null)
        {
            context.Fail("Invalid token user");
            return;
        }

        var identity = context.Principal!.Identity as ClaimsIdentity;

        identity?.AddClaim(new Claim(ClaimTypes.Role, userDTO.Data.Role!));
        
        //foreach (var role in roles)
        //{
        //    if (!identity!.HasClaim(ClaimTypes.Role, role))
        //        identity.AddClaim(new Claim(ClaimTypes.Role, role));
        //}
        //// Additional validation logic can be added here, such as checking if the user exists in the database.
        //return Task.CompletedTask;
        }
    }
