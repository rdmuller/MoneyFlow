using MoneyFlow.Application.DTOs.General.Sectors;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Sectors.Queries.GetAll;

public sealed class GetAllSectorsQuery : QueryList<SectorQueryDTO>;
