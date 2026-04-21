using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyFlow.API.APIs.Models;
using MoneyFlow.Application.DTOs.General.Sectors;
using MoneyFlow.Application.UseCases.General.Sectors.Commands.Create;
using MoneyFlow.Application.UseCases.General.Sectors.Commands.Delete;
using MoneyFlow.Application.UseCases.General.Sectors.Commands.Update;
using MoneyFlow.Application.UseCases.General.Sectors.Queries.GetAll;
using MoneyFlow.Application.UseCases.General.Sectors.Queries.GetByExternalId;
using MoneyFlow.Domain.General.Enums;
using SharedKernel.Communications;
using SharedKernel.Mediator;
using Swashbuckle.AspNetCore.Annotations;

namespace MoneyFlow.API.Controllers.General;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = Roles.ADMIN_OR_USER)]
public class SectorsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get list",
        Description = "Retorna lista de setores"
    )]
    [ProducesResponseType(typeof(BaseQueryResponse<IEnumerable<SectorQueryDTO>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAll([FromQuery] BoundQueryParams queryParams)
    {
        var result = await _mediator.SendAsync(new GetAllSectorsQuery { Query = queryParams });

        return result.IsSuccess ? Ok(result.Value) : NoContent();
    }

    [HttpGet("{externalId}")]
    [SwaggerOperation(
        Summary = "Get by id",
        Description = "Retorna todos os dados de um setor"
    )]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseResponse<SectorQueryDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid externalId)
    {
        var result = await _mediator.SendAsync(new GetSectorByExternalIdQuery(externalId));

        return result.IsSuccess ? Ok(BaseResponse<SectorQueryDTO>.CreateSuccessResponse(result.Value)) : NoContent();
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create",
        Description = "Cria um novo setor"
    )]
    [Authorize(Policy = Roles.ADMIN)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] BaseRequest<SectorCommandDTO> request)
    {
        var result = await _mediator.SendAsync(new CreateSectorCommand(request.Data?.Name ?? string.Empty, request.Data!.CategoryExternalId));

        if (result.IsFailure)
            return BadRequest(BaseResponse<string>.CreateFailureResponse(result.Errors!));

        return Created("", BaseResponse<string>.CreateNewObjectIdResponse(result.Value));
    }

    [HttpPut("{externalId}")]
    [SwaggerOperation(
        Summary = "Update",
        Description = "Atualiza os dados de um setor"
    )]
    [Authorize(Policy = Roles.ADMIN)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid externalId, [FromBody] BaseRequest<SectorCommandDTO> request)
    {
        var result = await _mediator.SendAsync(new UpdateSectorCommand(externalId, request.Data?.Name, request.Data?.CategoryExternalId, request.Data?.Active));

        if (result.IsFailure)
            return BadRequest(BaseResponse<string>.CreateFailureResponse(result.Errors!));

        return NoContent();
    }

    [HttpDelete("{externalId}")]
    [SwaggerOperation(
        Summary = "Delete",
        Description = "Exclui um setor"
    )]
    [Authorize(Policy = Roles.ADMIN)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(Guid externalId)
    {
        var result = await _mediator.SendAsync(new DeleteSectorCommand(externalId));

        if (result.IsFailure)
            return BadRequest(BaseResponse<string>.CreateFailureResponse(result.Errors!));

        return NoContent();
    }
}
