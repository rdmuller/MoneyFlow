using Mediator.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyFlow.API.APIs.Models;
using MoneyFlow.Application.DTOs.General.Categories;
using MoneyFlow.Application.UseCases.General.Categories.Commands.Create;
using MoneyFlow.Application.UseCases.General.Categories.Commands.Update;
using MoneyFlow.Application.UseCases.General.Categories.Queries.GetAll;
using MoneyFlow.Application.UseCases.General.Categories.Queries.GetByExternalId;
using SharedKernel.Communications;
using Swashbuckle.AspNetCore.Annotations;

namespace MoneyFlow.API.Controllers.General;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoriesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(BaseResponse<IEnumerable<CategoryQueryDTO>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAll([FromQuery] BoundQueryParams queryParams)
    {
        var result = await _mediator.SendAsync(new GetAllCategoriesQuery { Query = queryParams });
        return Ok(result);
    }


    [SwaggerOperation(
        Summary = "Consultar uma categoria",
        Description = "Retorna todos os dados de uma categoria",
        Tags = new[] { "Categoria" }
    )]
    [HttpGet("{externalId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseResponse<CategoryQueryDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid externalId)
    {
        var result = await _mediator.SendAsync(new GetSectorByExternalIdQuery(externalId));

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] BaseRequest<CategoryCommandDTO> request)
    {
        var result = await _mediator.SendAsync(new CreateCategoryCommand(request.Data?.Name));

        return Created("", result);
    }

    [HttpPut("{externalId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid externalId, [FromBody] BaseRequest<CategoryCommandDTO> request)
    {
        await _mediator.SendAsync(new UpdateCategoryCommand(externalId, request.Data?.Name, request.Data?.Active));

        return NoContent();
    }
}
