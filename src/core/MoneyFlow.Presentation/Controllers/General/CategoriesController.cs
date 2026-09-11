using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyFlow.Domain.General.Enums;
using Shared.Presentation.Communications;
using Swashbuckle.AspNetCore.Annotations;
using Shared.Application.Messaging;
using MoneyFlow.Application.UseCases.General.Categories.Commands.Create;
using Shared.Domain;
using MoneyFlow.Application.UseCases.General.Categories.Commands.Update;
using MoneyFlow.Application.DTOs.General.Categories;
using MoneyFlow.Application.UseCases.General.Categories.Commands.Delete;
using MoneyFlow.Application.UseCases.General.Categories.Queries.GetByExternalId;
using Shared.Presentation.APIs.Models;
using MoneyFlow.Application.UseCases.General.Categories.Queries.GetAll;

namespace MoneyFlow.Presentation.Controllers.General;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get list",
        Description = "Retorna lista de categorias"
    )]
    [Authorize(Policy = Roles.ADMIN_OR_USER)]
    [ProducesResponseType(typeof(BaseQueryResponse<IEnumerable<CategoryQueryDTO>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAll(
        [FromQuery] BoundQueryParams queryParams, 
        [FromServices] IQueryHandler<GetAllCategoriesQuery, IReadOnlyList<CategoryQueryDTO>> handler)
    {
        Result<IReadOnlyList<CategoryQueryDTO>> result = await handler.HandleAsync(new GetAllCategoriesQuery { Query = queryParams });

        return result.IsSuccess ? Ok(BaseResponse<IReadOnlyList<CategoryQueryDTO>>.CreatePaginatedResponse(result)) : NoContent();
    }

    [HttpGet("{externalId}")]
    [SwaggerOperation(
        Summary = "Get by id",
        Description = "Retorna todos os dados de uma categoria"
    )]
    [Authorize(Policy = Roles.ADMIN_OR_USER)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseResponse<CategoryQueryDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(
        Guid externalId,
        [FromServices] IQueryHandler<GetCategoryByExternalIdQuery, CategoryQueryDTO> handler)
    {
        Result<CategoryQueryDTO> result = await handler.HandleAsync(new GetCategoryByExternalIdQuery(externalId));

        return result.IsSuccess ? Ok(BaseResponse<CategoryQueryDTO>.CreateSuccessResponse(result.Value)) : NoContent();
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create",
        Description = "Cria uma nova categoria"
    )]
    [Authorize(Policy = Roles.ADMIN)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromServices] ICommandHandler<CreateCategoryCommand, Guid> handler,
        [FromBody] BaseRequest<CreateCategoryCommand> command)
    {
        Result<Guid> result = await handler.HandleAsync(command.Data);

        if (result.IsFailure)
            return BadRequest(BaseResponse<string>.CreateFailureResponse(result.Errors!));

        return Created("", BaseResponse<string>.CreateNewObjectIdResponse(result.Value));
    }

    [HttpPut("{externalId}")]
    [SwaggerOperation(
        Summary = "Update",
        Description = "Atualiza os dados de uma categoria"
    )]
    [Authorize(Policy = Roles.ADMIN)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(
        Guid externalId, 
        [FromServices] ICommandHandler<UpdateCategoryCommand> handler,
        [FromBody] BaseRequest<CategoryCommandDTO> command)
    {
        Result result = await handler.HandleAsync(new UpdateCategoryCommand(externalId, command.Data?.Name, command.Data?.Active));

        if (result.IsFailure)
            return BadRequest(BaseResponse<string>.CreateFailureResponse(result.Errors!));

        return NoContent();
    }

    [HttpDelete("{externalId}")]
    [SwaggerOperation(
        Summary = "Delete",
        Description = "Exclui uma categoria"
    )]
    [Authorize(Policy = Roles.ADMIN)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(
        [FromServices] ICommandHandler<DeleteCategoryCommand> handler,
        Guid externalId)
    {
        Result result = await handler.HandleAsync(new DeleteCategoryCommand(externalId));

        if (result.IsFailure)
            return BadRequest(BaseResponse<string>.CreateFailureResponse(result.Errors!));

        return NoContent();
    }
}
