using MoneyFlow.Application.DTOs.General.Users;
using Shared.Application.Messaging;

namespace MoneyFlow.Application.UseCases.General.Users.Queries.GetLoggedUserProfile;

public sealed record GetLoggedUserProfileQuery : IQuery<GetUserFullQueryDTO>;
