using Mediator.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyFlow.Application.DTOs.Common.Markets;
using MoneyFlow.Application.UseCases.Common.Markets.Commands.Create;
using SharedKernel.Communications;

namespace MoneyFlow.API.Controllers.Common;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MarketsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> CreateMarket([FromBody] BaseRequest<MarketCommandDTO> request)
    {
        var result = await _mediator.SendAsync(new CreateMarketCommand { Market = request.Data });

        return Created("", result);
    }
}
