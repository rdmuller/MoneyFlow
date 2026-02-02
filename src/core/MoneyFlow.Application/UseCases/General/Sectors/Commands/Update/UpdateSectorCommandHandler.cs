using Mediator.Abstractions;
using MoneyFlow.Domain.Abstractions;
using MoneyFlow.Domain.General.Entities.Categories;
using MoneyFlow.Domain.General.Entities.Sectors;
using SharedKernel.Communications;
using SharedKernel.Exceptions;

namespace MoneyFlow.Application.UseCases.General.Sectors.Commands.Update;

internal class UpdateSectorCommandHandler(
    ISectorWriteRepository sectorWriteRepository,
    ICategoryReadRepository categoryReadRepository,
    IUnitOfWork unitOfWork) : IHandler<UpdateSectorCommand, BaseResponse<string>>
{
    private readonly ISectorWriteRepository _sectorWriteRepository = sectorWriteRepository;
    private readonly ICategoryReadRepository _categoryReadRepository = categoryReadRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;


    public async Task<BaseResponse<string>> HandleAsync(UpdateSectorCommand request, CancellationToken cancellationToken = default)
    {
        if (!request.ExternalId.HasValue)
            throw ErrorOnValidationException.RequiredFieldIsEmpty("Sector is required");

        var sector = await _sectorWriteRepository.GetByExternalIdAsync(request.ExternalId.Value);
        if (sector is null)
            throw DataBaseException.RecordNotFound("Sector not found");

        var category = await _categoryReadRepository.GetByExternalIdAsync(request.CategoryId ?? Guid.Empty, cancellationToken);
        if (category is null)
            throw DataBaseException.RecordNotFound("Category not found");

        sector.Update(request.Name!, category!, (bool)request.Active!);

        _sectorWriteRepository.Update(sector);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new BaseResponse<string>();
    }
}
