using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyFlow.API.APIs.Models;
using MoneyFlow.Application.DTOs.General.Markets;
using MoneyFlow.Application.UseCases.General.Markets.Commands.Create;
using MoneyFlow.Application.UseCases.General.Markets.Commands.Delete;
using MoneyFlow.Application.UseCases.General.Markets.Commands.Update;
using MoneyFlow.Application.UseCases.General.Markets.Queries.GetAll;
using MoneyFlow.Application.UseCases.General.Markets.Queries.GetByExternalId;
using MoneyFlow.Domain.General.Enums;
using SharedKernel.Communications;
using SharedKernel.Mediator;
using Swashbuckle.AspNetCore.Annotations;

namespace MoneyFlow.API.Controllers.General;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = Roles.ADMIN_OR_USER)]
public class MarketsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get list",
        Description = "Retorna lista de mercados"
    )]
    [ProducesResponseType(typeof(BaseQueryResponse<IEnumerable<MarketQueryDTO>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAll([FromQuery] BoundQueryParams queryParams)
    {
        var result = await _mediator.SendAsync(new GetAllMarketsQuery { Query = queryParams });

        return result.IsSuccess ? Ok(result.Value) : NoContent();
    }


    [HttpGet("{externalId}")]
    [SwaggerOperation(
        Summary = "Get by id",
        Description = "Retorna todos os dados de um mercado"
    )]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseResponse<MarketQueryDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid externalId)
    {
        var result = await _mediator.SendAsync(new GetMarketByExternalIdQuery(externalId));

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
    public async Task<IActionResult> Create([FromBody] BaseRequest<MarketCommandDTO> request)
    {
        var result = await _mediator.SendAsync(new CreateMarketCommand(request.Data?.Name));

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
    public async Task<IActionResult> Update(Guid externalId, [FromBody] BaseRequest<MarketCommandDTO> request)
    {
        var result = await _mediator.SendAsync(new UpdateMarketCommand(externalId, request.Data?.Name, request.Data?.Active));

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
    public async Task<IActionResult> Delete(Guid externalId)
    {
        var result = await _mediator.SendAsync(new DeleteMarketCommand(externalId));

        if (result.IsFailure)
            return BadRequest(BaseResponse<string>.CreateFailureResponse(result.Errors!));

        return NoContent();
    }
}
