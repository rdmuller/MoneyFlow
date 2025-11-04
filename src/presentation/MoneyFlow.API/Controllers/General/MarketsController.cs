using Mediator.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyFlow.Application.DTOs.Common.Markets;
using MoneyFlow.Application.UseCases.General.Markets.Commands.Create;
using MoneyFlow.Application.UseCases.General.Markets.Commands.Update;
using MoneyFlow.Application.UseCases.General.Markets.Queries.GetAll;
using MoneyFlow.Application.UseCases.General.Markets.Queries.GetById;
using SharedKernel.Communications;
using System.Net;

namespace MoneyFlow.API.Controllers.Common;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MarketsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateMarket([FromBody] BaseRequest<MarketCommandDTO> request)
    {
        var result = await _mediator.SendAsync(new CreateMarketCommand { Market = request.Data });

        return Created("", result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateMarket(long id, [FromBody] BaseRequest<MarketCommandDTO> request)
    {
        request.Data!.Id = id;

        var result = await _mediator.SendAsync(new UpdateMarketCommand { Market = request.Data });

        return NoContent();
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BaseResponse<MarketQueryDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetMarketById(long id)
    {
        var result = await _mediator.SendAsync(new GetMarketByIdQuery { Id = id });
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(BaseQueryResponse<List<MarketQueryDTO>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAllMarkets([FromQuery]QueryParams query)
    {
        var result = await _mediator.SendAsync(new GetAllMarketsQuery { Query = query });
        return Ok(result);
    }
}
