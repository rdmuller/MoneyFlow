using MoneyFlow.Application.DTOs.General.Users;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Users.Queries.GetLoggedUserProfile;

public sealed record GetLoggedUserProfileQuery : IQuery<GetUserFullQueryDTO>;
