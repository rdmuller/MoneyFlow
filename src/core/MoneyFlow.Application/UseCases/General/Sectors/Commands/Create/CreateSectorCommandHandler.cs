using MoneyFlow.Domain.Abstractions.DataAccess;
using MoneyFlow.Domain.General.Entities.Categories;
using MoneyFlow.Domain.General.Entities.Sectors;
using SharedKernel.Communications;
using SharedKernel.Exceptions;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Sectors.Commands.Create;

public class CreateSectorCommandHandler(
    ICategoryReadRepository categoryReadRepository,
    ISectorWriteRepository sectorWriteRepository,
    IUnitOfWork unitOfWork) : IHandler<CreateSectorCommand, BaseResponse<string>>
{
    private readonly ICategoryReadRepository _categoryReadRepository = categoryReadRepository;
    private readonly ISectorWriteRepository _sectorWriteRepository = sectorWriteRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<string>> HandleAsync(CreateSectorCommand request, CancellationToken cancellationToken = default)
    {
        var category = await _categoryReadRepository.GetByExternalIdAsync(request.categoryExternalId, cancellationToken);
        if (category is null)
            throw DataBaseException.RecordNotFound($"Category not found.");

        var sector = Sector.Create(request.Name, category);

        if (sector.IsFailure)
            return BaseResponse<string>.CreateFailureResponse(sector.Errors!);

        await _sectorWriteRepository.CreateAsync(sector.Value, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return BaseResponse<string>.CreateNewObjectIdResponse(sector.Value.ExternalId);
    }
}
