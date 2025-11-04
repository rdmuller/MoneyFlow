using Mediator.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyFlow.Application.UseCases.General.Categories.Queries;

namespace MoneyFlow.API.Controllers.General;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoriesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById(long id)
    {
        var result = await _mediator.SendAsync(new GetCategoryByIdQuery { Id = id });

        return Ok(result);
    }

}
