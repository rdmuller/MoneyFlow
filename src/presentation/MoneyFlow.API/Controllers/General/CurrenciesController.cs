using Mediator.Abstractions;
using Microsoft.AspNetCore.Mvc;
using MoneyFlow.Application.DTOs.General.Currencies;
using MoneyFlow.Application.UseCases.General.Currencies.Commands.Create;
using SharedKernel.Communications;
using Swashbuckle.AspNetCore.Annotations;

namespace MoneyFlow.API.Controllers.General;

[Route("api/[controller]")]
[ApiController]
public class CurrenciesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [SwaggerOperation(
        Summary = "Incluir moeda",
        Description = "Incluir moeda",
        OperationId = "Insert"
    //Tags = new[] { "Setor" }
    )]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody]BaseRequest<CurrencyCommandDTO> request)
    {
        var result = await _mediator.SendAsync(new CreateCurrencyCommand(request.Data?.Name, request.Data?.Symbol));

        return Created("", result);
    }
}
