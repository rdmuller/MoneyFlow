using Mediator.Abstractions;
using MoneyFlow.Domain.Abstractions;
using MoneyFlow.Domain.General.Entities.Categories;
using SharedKernel.Communications;

namespace MoneyFlow.Application.UseCases.General.Categories.Commands.Create;

internal class CreateCategoryCommandHandler(ICategoryWriteRepository categoryWriteRepository, IUnitOfWork unitOfWork) : IHandler<CreateCategoryCommand, BaseResponse<string>>
{
    private readonly ICategoryWriteRepository _categoryWriteRepository = categoryWriteRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<string>> HandleAsync(CreateCategoryCommand request, CancellationToken cancellationToken = default)
    {
        var category = Category.Create(request.Name);

        await _categoryWriteRepository.CreateAsync(category, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new BaseResponse<string>
        {
            ObjectId = category.ExternalId,
        };
    }
}
