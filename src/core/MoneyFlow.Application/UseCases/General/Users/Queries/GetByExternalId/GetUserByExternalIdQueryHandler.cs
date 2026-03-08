using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.General.Users;
using MoneyFlow.Domain.General.Entities.Users;
using SharedKernel.Communications;
using SharedKernel.Exceptions;

namespace MoneyFlow.Application.UseCases.General.Users.Queries.GetByExternalId;

internal class GetUserByExternalIdQueryHandler(IUserReadRepository userReadRepository) : IHandler<GetUserByExternalIdQuery, BaseResponse<GetUserFullQueryDTO>>
{
    private readonly IUserReadRepository _userReadRepository = userReadRepository;
    public async Task<BaseResponse<GetUserFullQueryDTO>> HandleAsync(GetUserByExternalIdQuery request, CancellationToken cancellationToken = default)
    {
        if (request.ExternalId.HasValue)
            throw new NoContentException();

        var user = await _userReadRepository.GetByExternalIdAsync((Guid)request.ExternalId!, cancellationToken);


        return BaseResponse<GetUserFullQueryDTO>.CreateSuccessResponse(null);
    }
}
