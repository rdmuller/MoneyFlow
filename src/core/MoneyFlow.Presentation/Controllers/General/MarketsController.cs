using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyFlow.Application.DTOs.General.Markets;
using MoneyFlow.Application.UseCases.General.Markets.Commands.Create;
using MoneyFlow.Application.UseCases.General.Markets.Commands.Delete;
using MoneyFlow.Application.UseCases.General.Markets.Commands.Update;
using MoneyFlow.Application.UseCases.General.Markets.Queries.GetAll;
using MoneyFlow.Application.UseCases.General.Markets.Queries.GetByExternalId;
using MoneyFlow.Domain.General.Enums;
using Shared.Application.Messaging;
using Shared.Domain;
using Shared.Presentation.APIs.Models;
using Shared.Presentation.Communications;
using Swashbuckle.AspNetCore.Annotations;

namespace MoneyFlow.Presentation.Controllers.General;

[Route("api/[controller]")]
[ApiController]
public class MarketsController : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get list",
        Description = "Retorna lista de mercados"
    )]
    [Authorize(Policy = Roles.ADMIN_OR_USER)]
    [ProducesResponseType(typeof(BaseQueryResponse<IEnumerable<MarketQueryDTO>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAll(
        [FromQuery] BoundQueryParams queryParams, 
        [FromServices] IQueryHandler<GetAllMarketsQuery, IReadOnlyList<MarketQueryDTO>> handler)
    {
        Result<IReadOnlyList<MarketQueryDTO>> result = await handler.HandleAsync(new GetAllMarketsQuery { Query = queryParams });

        return result.IsSuccess ? Ok(BaseResponse<IReadOnlyList<MarketQueryDTO>>.CreatePaginatedResponse(result)) : NoContent();
    }

    [HttpGet("{externalId}")]
    [SwaggerOperation(
        Summary = "Get by id",
        Description = "Retorna todos os dados de um mercado"
    )]
    [Authorize(Policy = Roles.ADMIN_OR_USER)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseResponse<MarketQueryDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(
        Guid externalId,
        [FromServices] IQueryHandler<GetMarketByExternalIdQuery, MarketQueryDTO> handler)
    {
        Result<MarketQueryDTO> result = await handler.HandleAsync(new GetMarketByExternalIdQuery(externalId));

        return result.IsSuccess ? Ok(BaseResponse<MarketQueryDTO>.CreateSuccessResponse(result.Value)) : NoContent();
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create",
        Description = "Cria um novo mercado"
    )]
    [Authorize(Policy = Roles.ADMIN)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromServices] ICommandHandler<CreateMarketCommand, Guid> handler,
        [FromBody] BaseRequest<CreateMarketCommand> command)
    {
        Result<Guid> result = await handler.HandleAsync(command.Data);

        if (result.IsFailure)
            return BadRequest(BaseResponse<string>.CreateFailureResponse(result.Errors!));

        return Created("", BaseResponse<string>.CreateNewObjectIdResponse(result.Value));
    }

    [HttpPut("{externalId}")]
    [SwaggerOperation(
        Summary = "Update",
        Description = "Atualiza os dados de um mercado"
    )]
    [Authorize(Policy = Roles.ADMIN)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(
        Guid externalId, 
        [FromServices] ICommandHandler<UpdateMarketCommand> handler,
        [FromBody] BaseRequest<MarketCommandDTO> command)
    {
        Result result = await handler.HandleAsync(new UpdateMarketCommand(externalId, command.Data?.Name, command.Data?.Active));

        if (result.IsFailure)
            return BadRequest(BaseResponse<string>.CreateFailureResponse(result.Errors!));

        return NoContent();
    }

    [HttpDelete("{externalId}")]
    [SwaggerOperation(
        Summary = "Delete",
        Description = "Exclui um mercado"
    )]
    [Authorize(Policy = Roles.ADMIN)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(
        [FromServices] ICommandHandler<DeleteMarketCommand> handler,
        Guid externalId)
    {
        Result result = await handler.HandleAsync(new DeleteMarketCommand(externalId));

        if (result.IsFailure)
            return BadRequest(BaseResponse<string>.CreateFailureResponse(result.Errors!));

        return NoContent();
    }
}

