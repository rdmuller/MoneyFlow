using MoneyFlow.Application.DTOs.General.Users;
using SharedKernel.Communications;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Users.Queries.GetByExternalId;

public sealed record GetUserByExternalIdQuery(Guid? ExternalId) : IRequest<BaseResponse<GetUserFullQueryDTO>>;
