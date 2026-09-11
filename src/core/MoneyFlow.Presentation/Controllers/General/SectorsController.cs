using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyFlow.Application.DTOs.General.Sectors;
using MoneyFlow.Application.UseCases.General.Sectors.Commands.Create;
using MoneyFlow.Application.UseCases.General.Sectors.Commands.Delete;
using MoneyFlow.Application.UseCases.General.Sectors.Commands.Update;
using MoneyFlow.Application.UseCases.General.Sectors.Queries.GetAll;
using MoneyFlow.Application.UseCases.General.Sectors.Queries.GetByExternalId;
using MoneyFlow.Domain.General.Enums;
using Shared.Application.Messaging;
using Shared.Domain;
using Shared.Presentation.APIs.Models;
using Shared.Presentation.Communications;
using Swashbuckle.AspNetCore.Annotations;

namespace MoneyFlow.Presentation.Controllers.General;

[Route("api/[controller]")]
[ApiController]
public class SectorsController : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get list",
        Description = "Retorna lista de setores"
    )]
    [Authorize(Policy = Roles.ADMIN_OR_USER)]
    [ProducesResponseType(typeof(BaseQueryResponse<IEnumerable<SectorQueryDTO>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAll(
        [FromQuery] BoundQueryParams queryParams, 
        [FromServices] IQueryHandler<GetAllSectorsQuery, IReadOnlyList<SectorQueryDTO>> handler)
    {
        Result<IReadOnlyList<SectorQueryDTO>> result = await handler.HandleAsync(new GetAllSectorsQuery { Query = queryParams });

        return result.IsSuccess ? Ok(BaseResponse<IReadOnlyList<SectorQueryDTO>>.CreatePaginatedResponse(result)) : NoContent();
    }

    [HttpGet("{externalId}")]
    [SwaggerOperation(
        Summary = "Get by id",
        Description = "Retorna todos os dados de um setor"
    )]
    [Authorize(Policy = Roles.ADMIN_OR_USER)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseResponse<SectorQueryDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(
        Guid externalId,
        [FromServices] IQueryHandler<GetSectorByExternalIdQuery, SectorQueryDTO> handler)
    {
        Result<SectorQueryDTO> result = await handler.HandleAsync(new GetSectorByExternalIdQuery(externalId));

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
    public async Task<IActionResult> Create(
        [FromServices] ICommandHandler<CreateSectorCommand, Guid> handler,
        [FromBody] BaseRequest<CreateSectorCommand> command)
    {
        Result<Guid> result = await handler.HandleAsync(command.Data);

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
    public async Task<IActionResult> Update(
        Guid externalId, 
        [FromServices] ICommandHandler<UpdateSectorCommand> handler,
        [FromBody] BaseRequest<SectorCommandDTO> command)
    {
        Result result = await handler.HandleAsync(new UpdateSectorCommand(externalId, command.Data?.Name, command.Data?.CategoryExternalId, command.Data?.Active));

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
    public async Task<IActionResult> Delete(
        [FromServices] ICommandHandler<DeleteSectorCommand> handler,
        Guid externalId)
    {
        Result result = await handler.HandleAsync(new DeleteSectorCommand(externalId));

        if (result.IsFailure)
            return BadRequest(BaseResponse<string>.CreateFailureResponse(result.Errors!));

        return NoContent();
    }
}

