using Mediator.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyFlow.API.APIs.Models;
using MoneyFlow.Application.DTOs.General.Categories;
using MoneyFlow.Application.UseCases.General.Categories.Commands.Create;
using MoneyFlow.Application.UseCases.General.Categories.Commands.Update;
using MoneyFlow.Application.UseCases.General.Categories.Queries.GetAll;
using MoneyFlow.Application.UseCases.General.Categories.Queries.GetById;
using SharedKernel.Communications;

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


    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseResponse<CategoryQueryDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _mediator.SendAsync(new GetCategoryByIdQuery { Id = id });

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] BaseRequest<CategoryCommandDTO> request)
    {
        var result = await _mediator.SendAsync(new CreateCategoryCommand { Category = request.Data! });

        return Created("", result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(long id, [FromBody] BaseRequest<CategoryCommandDTO> request)
    {
        request.Data!.Id = id;

        await _mediator.SendAsync(new UpdateCategoryCommand { Category = request.Data! });

        return NoContent();
    }
}
