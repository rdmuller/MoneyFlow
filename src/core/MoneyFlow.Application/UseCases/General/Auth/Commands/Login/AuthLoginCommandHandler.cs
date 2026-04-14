using MoneyFlow.Application.DTOs.General.Auth;
using MoneyFlow.Domain.General.Entities.Users;
using MoneyFlow.Domain.General.Security;
using SharedKernel.Abstractions;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Auth.Commands.Login;

public class AuthLoginCommandHandler(
    IUserReadRepository userQueryRepository,
    IPasswordHasher passwordHasher,
    IAccessTokenGenerator accessTokenGenerator) : ICommandHandler<AuthLoginCommand, TokenDTO>
{
    private readonly IUserReadRepository _userQueryRepository = userQueryRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IAccessTokenGenerator _accessTokenGenerator = accessTokenGenerator;

    public async Task<Result<TokenDTO>> HandleAsync(AuthLoginCommand request, CancellationToken cancellationToken = default)
    {
        var user = await ValidateLogin(request);

        if (user.IsFailure)
            return Result.Failure<TokenDTO>(user.Errors!);

        var token = _accessTokenGenerator.GenerateAccessToken(user.Value);
        return new TokenDTO
        {
            Token = token.Token,
            ExpiresAt = token.ExpiresAt
        };
    }

    private async Task<Result<User>> ValidateLogin(AuthLoginCommand request)
    {
        /* var errors = await new AuthLoginValidator().ValidateWithErrorsAsync(request);

         if (errors.Count > 0)
             return Result.Failure<User>(errors);*/

        var user = await _userQueryRepository.GetByEmailAsync(new Email(request.Email));

        if (user is null)
            return Result.Failure<User>(Error.NotAuthorized("Invalid e-mail"));

        if (!_passwordHasher.Verify(request.Password, user.Password))
            return Result.Failure<User>(Error.NotAuthorized("Invalid password"));

        return Result.Success(user);
    }
}
