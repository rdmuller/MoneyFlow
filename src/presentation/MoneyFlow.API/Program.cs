using Mediator.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using MoneyFlow.API.Security;
using MoneyFlow.Application;
using MoneyFlow.Application.Common.Behaviors;
using MoneyFlow.Domain.Security;
using MoneyFlow.Infra;
using MoneyFlow.Infra.DataAccess;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfra(builder.Configuration);
builder.Services.AddDependencyInjectionApplication();
builder.Services.AddMediator(typeof(MoneyFlow.Application.DependencyInjection).Assembly);
builder.Services.AddScoped<ITokenProvider, HttpContextTokenValue>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers(options => {
    //options.Filters.Add<ValidationFilter>();
    options.Filters.Add<ExceptionFilter>();
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();

var signingKey = builder.Configuration.GetValue<string>("Settings:jwt:SigningKey");
builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = new TimeSpan(0),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey!))
    };
});

var app = builder.Build();

app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    AllowCachingResponses = false,
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
    }
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options => 
    {
        options.WithTheme(ScalarTheme.Moon)
            .WithDarkMode(true)
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
            .WithDarkModeToggle(false)
            .WithPreferredScheme(JwtBearerDefaults.AuthenticationScheme)
            .WithHttpBearerAuthentication(bearer =>
            {
                bearer.Token = "your-bearer-token";
            });
        options.Authentication = new ScalarAuthenticationOptions
        {
            PreferredSecurityScheme = "BearerAuth",
        };
    });
}

app.MapControllers();

app.Run();
