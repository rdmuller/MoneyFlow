using MoneyFlow.Domain.Abstractions.DataAccess;
using MoneyFlow.Domain.General.Entities.Categories;
using SharedKernel.Communications;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Categories.Commands.Create;

internal class CreateCategoryCommandHandler(ICategoryWriteRepository categoryWriteRepository, IUnitOfWork unitOfWork) : IHandler<CreateCategoryCommand, BaseResponse<string>>
{
    private readonly ICategoryWriteRepository _categoryWriteRepository = categoryWriteRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<string>> HandleAsync(CreateCategoryCommand request, CancellationToken cancellationToken = default)
    {
        var category = Category.Create(request.Name);

        if (category.IsFailure)
            return BaseResponse<string>.CreateFailureResponse(category.Errors!);

        await _categoryWriteRepository.CreateAsync(category.Value, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return BaseResponse<string>.CreateNewObjectIdResponse(category.Value.ExternalId);
    }
}
