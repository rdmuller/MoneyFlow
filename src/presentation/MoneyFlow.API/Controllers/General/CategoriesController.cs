using Mediator.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyFlow.Application.DTOs.General.Categories;
using MoneyFlow.Application.UseCases.General.Categories.Commands.Create;
using MoneyFlow.Application.UseCases.General.Categories.Queries;
using SharedKernel.Communications;

namespace MoneyFlow.API.Controllers.General;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoriesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseResponse<CategoryQueryDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategoryById(long id)
    {
        var result = await _mediator.SendAsync(new GetCategoryByIdQuery { Id = id });

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCategory([FromBody] BaseRequest<CategoryCommandDTO> request)
    {
        var result = await _mediator.SendAsync(new CreateCategoryCommand { Category = request.Data! });

        return Created("", result);
    }
}
