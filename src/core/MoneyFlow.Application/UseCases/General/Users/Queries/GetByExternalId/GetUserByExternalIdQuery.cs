using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.General.Users;
using SharedKernel.Communications;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyFlow.Application.UseCases.General.Users.Queries.GetByExternalId;

public sealed record GetUserByExternalIdQuery(Guid? ExternalId) : IRequest<BaseResponse<GetUserFullQueryDTO>>;
