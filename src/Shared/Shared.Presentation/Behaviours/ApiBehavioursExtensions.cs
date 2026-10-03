using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shared.Presentation.Communications;
using Shared.Presentation.Filters;

namespace Shared.Presentation.Behaviours;

public static class ApiBehavioursExtensions
{
    public static IServiceCollection AddControllersWithBehaviours(
        this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add<ValidationFilter>();
            options.Filters.Add<ExceptionFilter>();
        })
        .ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .SelectMany(x => x.Value!.Errors)
                    .Select(error => new Domain.Error("ValidationError", error.ErrorMessage)).ToList();

                return new BadRequestObjectResult(BaseResponse<object>.CreateFailureResponse(errors));
            };
        });

        return services;
    }
}
