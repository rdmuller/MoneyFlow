using Mediator.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using MoneyFlow.API.Security;
using MoneyFlow.Application;
using MoneyFlow.Application.Common.Behaviors;
using MoneyFlow.Domain.General.Security;
using MoneyFlow.Infra;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
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
//builder.Services.AddOpenApi();


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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config => 
{
    config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Type = SecuritySchemeType.Http,
        Description = "Bearer {token}"
    });

    config.EnableAnnotations();
});

builder.Services.AddOpenTelemetry()
    .ConfigureResource(r => r.AddService(nameof(MoneyFlow.API)))
    .WithMetrics(m => m
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddOtlpExporter())
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddEntityFrameworkCoreInstrumentation()
        .AddOtlpExporter()
//.AddOtlpExporter(opt => opt.Endpoint = new Uri("http://moneyflow.dashboard:18889"))
//.AddOtlpExporter(opt => opt.Endpoint = new Uri("http://otel-collector:4317"))
    );

builder.Logging.AddOpenTelemetry(logging => logging.AddOtlpExporter());

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c => c.RouteTemplate = "openapi/{documentName}.json");
    app.MapScalarApiReference(endpointPrefix: "/docs", options =>
    {
        options.WithTheme(ScalarTheme.BluePlanet)
            .WithTitle("MoneyFlow")
            .ForceDarkMode()
            .SortOperationsByMethod()
            .WithOpenApiRoutePattern("/openapi/{documentName}.json")
            ;
    });
}

app.MapControllers();

app.Run();
