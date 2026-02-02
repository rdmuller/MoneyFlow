using Mediator.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyFlow.Application.DTOs.General.Sectors;
using MoneyFlow.Application.UseCases.General.Sectors.Commands.Create;
using MoneyFlow.Application.UseCases.General.Sectors.Commands.Update;
using SharedKernel.Communications;
using Swashbuckle.AspNetCore.Annotations;

namespace MoneyFlow.API.Controllers.General;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SectorsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [SwaggerOperation(
        Summary = "Incluir setor",
        Description = "Incluir um novo setor vinculado a categoria",
        Tags = new[] { "Setor" }
    )]
    [HttpPost]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] BaseRequest<SectorCommandDTO> request)
    {
        var result = await _mediator.SendAsync(new CreateSectorCommand(request.Data?.Name ?? string.Empty, request.Data!.CategoryExternalId));
        return Created("", result);
    }

    [SwaggerOperation(
        Summary = "Alterar setor",
        Description = "Alterar um setor",
        Tags = new[] { "Setor" }
    )]
    [HttpPut("{externalId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid externalId, [FromBody] BaseRequest<SectorCommandDTO> request)
    {
        var result = await _mediator.SendAsync(new UpdateSectorCommand(externalId, request.Data?.Name, request.Data?.CategoryExternalId, request.Data?.Active));

        return NoContent();
    }
}
