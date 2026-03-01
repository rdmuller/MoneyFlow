using Mediator.Abstractions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MoneyFlow.API.APIs.Models;
using MoneyFlow.Application.DTOs.General.Currencies;
using MoneyFlow.Application.UseCases.General.Currencies.Commands.Create;
using MoneyFlow.Application.UseCases.General.Currencies.Queries.GetAll;
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

    [HttpGet]
    [SwaggerOperation(
        Summary = "Listar moedas",
        Description = "Listar moedas",
        OperationId = "GetAll"
    //Tags = new[] { "Setor" }
    )]
    [ProducesResponseType(typeof(BaseQueryResponse<IEnumerable<CurrencyQueryDTO>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAll([FromQuery]BoundQueryParams queryParams)
    {
        var result = await _mediator.SendAsync(new GetAllCurrenciesQuery()
        {
            Query = queryParams
        });

        return Ok(result);
    }
}
