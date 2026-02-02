using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.General.Sectors;
using SharedKernel.Communications;

namespace MoneyFlow.Application.UseCases.General.Sector.Queries.GetByExternalId;

public sealed record GetSectorByExternalIdQuery(Guid ExternalId) : IRequest<BaseResponse<SectorQueryDTO>>;