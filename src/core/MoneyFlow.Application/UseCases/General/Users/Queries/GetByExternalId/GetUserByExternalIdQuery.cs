using MoneyFlow.Application.DTOs.General.Users;
using Shared.Application.Messaging;

namespace MoneyFlow.Application.UseCases.General.Users.Queries.GetByExternalId;

public sealed record GetUserByExternalIdQuery(Guid? ExternalId) : IQuery<GetUserFullQueryDTO>;
