using MoneyFlow.Application.DTOs.General.Users;
using MoneyFlow.Domain.General.Entities.Users;
using SharedKernel.Communications;
using SharedKernel.Exceptions;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Users.Queries.GetByExternalId;

internal class GetUserByExternalIdQueryHandler(IUserReadRepository userReadRepository) : IHandler<GetUserByExternalIdQuery, BaseResponse<GetUserFullQueryDTO>>
{
    private readonly IUserReadRepository _userReadRepository = userReadRepository;
    public async Task<BaseResponse<GetUserFullQueryDTO>> HandleAsync(GetUserByExternalIdQuery request, CancellationToken cancellationToken = default)
    {
        if (!request.ExternalId.HasValue)
            throw ErrorOnValidationException.RequiredFieldIsEmpty($"ExternalId is required.");

        var user = await _userReadRepository.GetByExternalIdAsync((Guid)request.ExternalId!, cancellationToken);
        if (user is null)
            throw DataBaseException.RecordNotFound($"User with ExternalId {request.ExternalId} not found.");

        return BaseResponse<GetUserFullQueryDTO>.CreateSuccessResponse(GetUserFullQueryDTO.EntityToDTO(user));
    }
}
