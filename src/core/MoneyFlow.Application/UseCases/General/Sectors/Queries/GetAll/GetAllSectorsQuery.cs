using MoneyFlow.Application.DTOs.General.Sectors;
using Shared.Application.Messaging;

namespace MoneyFlow.Application.UseCases.General.Sectors.Queries.GetAll;

public sealed class GetAllSectorsQuery : QueryList<SectorQueryDTO>;
