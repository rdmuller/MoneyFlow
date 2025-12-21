using Mediator.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MoneyFlow.API.Security;
using MoneyFlow.Application;
using MoneyFlow.Application.Common.Behaviors;
using MoneyFlow.Domain.General.Security;
using MoneyFlow.Infra;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfra(builder.Configuration);
builder.Services.AddDependencyInjectionApplication();
builder.Services.AddMediator(typeof(MoneyFlow.Application.DependencyInjection).Assembly);
builder.Services.AddScoped<ITokenProvider, HttpContextTokenValue>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers(options =>
{
    //options.ModelBinderProviders.Insert(0, new QueryParamsBinderProvider()); // se ficar assim, não é necessário adicionar no modelo, ex: QueryParamsBinder
    options.Filters.Add<ValidationFilter>();
    options.Filters.Add<ExceptionFilter>();
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


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

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTheme(ScalarTheme.BluePlanet)
            .WithTitle("MoneyFlow")
            .ForceDarkMode()
            //.ExpandAllTags()
            .SortOperationsByMethod()
            .AddPreferredSecuritySchemes("BearerAuth")
            .AddHttpAuthentication("BearerAuth", auth =>
            {
                auth.Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...";
            });
    });
}

app.MapControllers();

app.Run();
