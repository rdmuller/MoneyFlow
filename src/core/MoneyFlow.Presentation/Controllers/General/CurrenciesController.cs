using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyFlow.Application.DTOs.General.Currencies;
using MoneyFlow.Application.UseCases.General.Currencies.Commands.Create;
using MoneyFlow.Application.UseCases.General.Currencies.Commands.Delete;
using MoneyFlow.Application.UseCases.General.Currencies.Commands.Update;
using MoneyFlow.Application.UseCases.General.Currencies.Queries.GetAll;
using MoneyFlow.Application.UseCases.General.Currencies.Queries.GetByExternalId;
using MoneyFlow.Domain.General.Enums;
using Shared.Application.Messaging;
using Shared.Domain;
using Shared.Presentation.APIs.Models;
using Shared.Presentation.Communications;
using Swashbuckle.AspNetCore.Annotations;

namespace MoneyFlow.Presentation.Controllers.General;

[Route("api/[controller]")]
[ApiController]
public class CurrenciesController : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get list",
        Description = "Retorna lista de moedas"
    )]
    [Authorize(Policy = Roles.ADMIN_OR_USER)]
    [ProducesResponseType(typeof(BaseQueryResponse<IEnumerable<CurrencyQueryDTO>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAll(
        [FromQuery] BoundQueryParams queryParams, 
        [FromServices] IQueryHandler<GetAllCurrenciesQuery, IReadOnlyList<CurrencyQueryDTO>> handler)
    {
        Result<IReadOnlyList<CurrencyQueryDTO>> result = await handler.HandleAsync(new GetAllCurrenciesQuery { Query = queryParams });

        return result.IsSuccess ? Ok(BaseResponse<IReadOnlyList<CurrencyQueryDTO>>.CreatePaginatedResponse(result)) : NoContent();
    }

    [HttpGet("{externalId}")]
    [SwaggerOperation(
        Summary = "Get by id",
        Description = "Retorna todos os dados de uma moeda"
    )]
    [Authorize(Policy = Roles.ADMIN_OR_USER)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseResponse<CurrencyQueryDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(
        Guid externalId,
        [FromServices] IQueryHandler<GetCurrencyByExternalIdQuery, CurrencyQueryDTO> handler)
    {
        Result<CurrencyQueryDTO> result = await handler.HandleAsync(new GetCurrencyByExternalIdQuery(externalId));

        return result.IsSuccess ? Ok(BaseResponse<CurrencyQueryDTO>.CreateSuccessResponse(result.Value)) : NoContent();
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create",
        Description = "Cria uma nova moeda"
    )]
    [Authorize(Policy = Roles.ADMIN)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromServices] ICommandHandler<CreateCurrencyCommand, Guid> handler,
        [FromBody] BaseRequest<CreateCurrencyCommand> command)
    {
        Result<Guid> result = await handler.HandleAsync(command.Data);

        if (result.IsFailure)
            return BadRequest(BaseResponse<string>.CreateFailureResponse(result.Errors!));

        return Created("", BaseResponse<string>.CreateNewObjectIdResponse(result.Value));
    }

    [HttpPut("{externalId}")]
    [SwaggerOperation(
        Summary = "Update",
        Description = "Atualiza os dados de uma moeda"
    )]
    [Authorize(Policy = Roles.ADMIN)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(
        Guid externalId, 
        [FromServices] ICommandHandler<UpdateCurrencyCommand> handler,
        [FromBody] BaseRequest<CurrencyCommandDTO> command)
    {
        Result result = await handler.HandleAsync(new UpdateCurrencyCommand(externalId, command.Data?.Name, command.Data?.Symbol, command.Data?.Active));

        if (result.IsFailure)
            return BadRequest(BaseResponse<string>.CreateFailureResponse(result.Errors!));

        return NoContent();
    }

    [HttpDelete("{externalId}")]
    [SwaggerOperation(
        Summary = "Delete",
        Description = "Exclui uma moeda"
    )]
    [Authorize(Policy = Roles.ADMIN)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(
        [FromServices] ICommandHandler<DeleteCurrencyCommand> handler,
        Guid externalId)
    {
        Result result = await handler.HandleAsync(new DeleteCurrencyCommand(externalId));

        if (result.IsFailure)
            return BadRequest(BaseResponse<string>.CreateFailureResponse(result.Errors!));

        return NoContent();
    }
}

