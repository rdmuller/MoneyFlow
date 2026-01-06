using Mediator.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyFlow.API.APIs.Models;
using MoneyFlow.Application.DTOs.General.Markets;
using MoneyFlow.Application.UseCases.General.Markets.Commands.Create;
using MoneyFlow.Application.UseCases.General.Markets.Commands.Update;
using MoneyFlow.Application.UseCases.General.Markets.Queries.GetAll;
using MoneyFlow.Application.UseCases.General.Markets.Queries.GetByExternalId;
using SharedKernel.Communications;

namespace MoneyFlow.API.Controllers.General;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MarketsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateMarket([FromBody] BaseRequest<MarketCommandDTO> request)
    {
        var result = await _mediator.SendAsync(new CreateMarketCommand(request.Data?.Name));

        return Created("", result);
    }

    [HttpPut("{externalId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateMarket(Guid externalId, [FromBody] BaseRequest<MarketCommandDTO> request)
    {
        var result = await _mediator.SendAsync(new UpdateMarketCommand(externalId, request.Data?.Name, request.Data?.Active));

        return NoContent();
    }

    [HttpGet("{externalId}")]
    [ProducesResponseType(typeof(BaseResponse<MarketQueryDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetMarketById(Guid externalId)
    {
        var result = await _mediator.SendAsync(new GetMarketByExternalIdQuery(externalId));
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(BaseQueryResponse<List<MarketQueryDTO>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAllMarkets([FromQuery] BoundQueryParams query)
    {
        var result = await _mediator.SendAsync(new GetAllMarketsQuery { Query = query });
        return Ok(result);
    }
}
