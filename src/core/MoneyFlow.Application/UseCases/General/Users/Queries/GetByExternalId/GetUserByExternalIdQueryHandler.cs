using MoneyFlow.Application.DTOs.General.Users;
using MoneyFlow.Domain.General.Entities.Users;
using SharedKernel.Abstractions;
using SharedKernel.Communications;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Users.Queries.GetByExternalId;

internal class GetUserByExternalIdQueryHandler(IUserReadRepository userReadRepository)
    : IQueryHandler<GetUserByExternalIdQuery, GetUserFullQueryDTO>
{
    private readonly IUserReadRepository _userReadRepository = userReadRepository;

    public async Task<Result<GetUserFullQueryDTO>> HandleAsync(GetUserByExternalIdQuery request, CancellationToken cancellationToken = default)
    {
        if (!request.ExternalId.HasValue)
            return Result.Failure<GetUserFullQueryDTO>(Error.RequiredFieldIsEmpty("ExternalId is required."));

        var user = await _userReadRepository.GetByExternalIdAsync((Guid)request.ExternalId!, cancellationToken);
        if (user is null)
            return Result.Failure<GetUserFullQueryDTO>(Error.RecordNotFound($"User with ExternalId {request.ExternalId} not found."));

        return Result<GetUserFullQueryDTO>.Create(GetUserFullQueryDTO.EntityToDTO(user));
    }
}
