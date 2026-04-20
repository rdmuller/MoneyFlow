using MoneyFlow.Application.DTOs.General.Sectors;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Sectors.Queries.GetByExternalId;

public sealed record GetSectorByExternalIdQuery(Guid ExternalId) : IQuery<SectorQueryDTO>;