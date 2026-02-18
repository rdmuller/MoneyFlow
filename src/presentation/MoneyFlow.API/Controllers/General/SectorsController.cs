using Mediator.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyFlow.API.APIs.Models;
using MoneyFlow.Application.DTOs.General.Sectors;
using MoneyFlow.Application.UseCases.General.Sectors.Commands.Create;
using MoneyFlow.Application.UseCases.General.Sectors.Commands.Update;
using MoneyFlow.Application.UseCases.General.Sectors.Queries.GetAll;
using MoneyFlow.Application.UseCases.General.Sectors.Queries.GetByExternalId;
using SharedKernel.Communications;
using Swashbuckle.AspNetCore.Annotations;

namespace MoneyFlow.API.Controllers.General;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SectorsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [SwaggerOperation(
        Summary = "Incluir setor",
        Description = "Incluir um novo setor vinculado a categoria",
        OperationId = "Insert"
        //Tags = new[] { "Setor" }
    )]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] BaseRequest<SectorCommandDTO> request)
    {
        var result = await _mediator.SendAsync(new CreateSectorCommand(request.Data?.Name ?? string.Empty, request.Data!.CategoryExternalId));
        return Created("", result);
    }

    [HttpPut("{externalId}")]
    [SwaggerOperation(
        Summary = "Alterar setor",
        Description = "Alterar um setor",
        OperationId = "Update"
        //Tags = new[] { "Setor" }
    )]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid externalId, [FromBody] BaseRequest<SectorCommandDTO> request)
    {
        var result = await _mediator.SendAsync(new UpdateSectorCommand(externalId, request.Data?.Name, request.Data?.CategoryExternalId, request.Data?.Active));

        return NoContent();
    }

    [HttpGet("{externalId}")]
    [SwaggerOperation(
        Summary = "Listar dados de um setor",
        Description = "Listar todos os dados de um setor",
        OperationId = "Get by id"
        //Tags = new[] { "Setor" }
    )]
    [ProducesResponseType(typeof(BaseResponse<SectorQueryDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetByExternalId(Guid externalId)
    {
        var result = await _mediator.SendAsync(new GetSectorByExternalIdQuery(externalId));
        return Ok(result);
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Listar setores",
        Description = "Listar todos setores",
        OperationId = "Get all"
        //Tags = new[] { "Setor" }
    )]
    [ProducesResponseType(typeof(BaseQueryResponse<SectorQueryDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAll([FromQuery]BoundQueryParams queryParams)
    {
        var result = await _mediator.SendAsync(new GetAllSectorsQuery { Query = queryParams });
        return Ok(result);
    }
}
