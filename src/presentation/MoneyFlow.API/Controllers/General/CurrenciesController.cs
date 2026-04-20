using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyFlow.API.APIs.Models;
using MoneyFlow.Application.DTOs.General.Currencies;
using MoneyFlow.Application.UseCases.General.Currencies.Commands.Create;
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

    [HttpPost]
    [Authorize(Policy = Roles.ADMIN)]
    [SwaggerOperation(
        Summary = "Incluir moeda",
        Description = "Incluir moeda",
        OperationId = "Insert"
    //Tags = new[] { "Setor" }
    )]
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
    [Authorize(Policy = Roles.ADMIN)]
    [SwaggerOperation(
        Summary = "Alterar moeda",
        Description = "Alterar moeda",
        OperationId = "Update")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid externalId, [FromBody] BaseRequest<CurrencyCommandDTO> request)
    {
        var result = await _mediator.SendAsync(new UpdateCurrencyCommand(externalId, request.Data?.Name, request.Data?.Symbol, request.Data?.Active));

        if (result.IsFailure)
            return BadRequest(BaseResponse<string>.CreateFailureResponse(result.Errors!));

        return NoContent();
    }


    [HttpGet]
    [SwaggerOperation(
        Summary = "Listar moedas",
        Description = "Listar moedas",
        OperationId = "GetAll"
    //Tags = new[] { "Setor" }
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
        Summary = "Dados de uma moeda",
        Description = "Consultar dados de uma moeda",
        OperationId = "Get"
    //Tags = new[] { "Setor" }
    )]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseResponse<CurrencyQueryDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid externalId)
    {
        var result = await _mediator.SendAsync(new GetCurrencyByExternalIdQuery(externalId));

        return result.IsSuccess ? Ok(BaseResponse<CurrencyQueryDTO>.CreateSuccessResponse(result.Value)) : NoContent();
    }
}
