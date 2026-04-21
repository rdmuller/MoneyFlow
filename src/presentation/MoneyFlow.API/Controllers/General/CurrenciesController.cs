using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyFlow.API.APIs.Models;
using MoneyFlow.Application.DTOs.General.Currencies;
using MoneyFlow.Application.UseCases.General.Currencies.Commands.Create;
using MoneyFlow.Application.UseCases.General.Currencies.Commands.Delete;
using MoneyFlow.Application.UseCases.General.Currencies.Commands.Update;
using MoneyFlow.Application.UseCases.General.Currencies.Queries.GetAll;
using MoneyFlow.Application.UseCases.General.Currencies.Queries.GetByExternalId;
using MoneyFlow.Domain.General.Enums;
using SharedKernel.Communications;
using SharedKernel.Mediator;
using Swashbuckle.AspNetCore.Annotations;

namespace MoneyFlow.API.Controllers.General;

[Route("api/[controller]")]
[Authorize(Policy = Roles.ADMIN_OR_USER)]
[ApiController]
public class CurrenciesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get list",
        Description = "Retorna lista de moedas"
    )]
    [ProducesResponseType(typeof(BaseQueryResponse<IEnumerable<CurrencyQueryDTO>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAll([FromQuery] BoundQueryParams queryParams)
    {
        var result = await _mediator.SendAsync(new GetAllCurrenciesQuery { Query = queryParams });

        return result.IsSuccess ? Ok(result.Value) : NoContent();
    }

    [HttpGet("{externalId}")]
    [SwaggerOperation(
        Summary = "Get by id",
        Description = "Retorna todos os dados de uma moeda"
    )]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseResponse<CurrencyQueryDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid externalId)
    {
        var result = await _mediator.SendAsync(new GetCurrencyByExternalIdQuery(externalId));

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
    public async Task<IActionResult> Create([FromBody] BaseRequest<CurrencyCommandDTO> request)
    {
        var result = await _mediator.SendAsync(new CreateCurrencyCommand(request.Data?.Name, request.Data?.Symbol));

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
    public async Task<IActionResult> Update(Guid externalId, [FromBody] BaseRequest<CurrencyCommandDTO> request)
    {
        var result = await _mediator.SendAsync(new UpdateCurrencyCommand(externalId, request.Data?.Name, request.Data?.Symbol, request.Data?.Active));

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
    public async Task<IActionResult> Delete(Guid externalId)
    {
        var result = await _mediator.SendAsync(new DeleteCurrencyCommand(externalId));

        if (result.IsFailure)
            return BadRequest(BaseResponse<string>.CreateFailureResponse(result.Errors!));

        return NoContent();
    }
}
