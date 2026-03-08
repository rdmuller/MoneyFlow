using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

namespace MoneyFlow.API.Security;

internal class JwtBearerEventsHandler : JwtBearerEvents
{
    public override async Task TokenValidated(TokenValidatedContext context)
    {
        var userExternalIdClaim = context.Principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
        if (userExternalIdClaim == null)
        {
            context.Fail("Invalid token");
            return;
        }
        //// Additional validation logic can be added here, such as checking if the user exists in the database.
        //return Task.CompletedTask;
    }
}
