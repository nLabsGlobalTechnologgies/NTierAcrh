﻿using AutoMapper;
using ErrorOr;
using MediatR;
using NTierAcrh.Entities.Repositories;

namespace NTierAcrh.Business.Features.Products.DeleteProduct;
internal sealed class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, ErrorOr<Unit>>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ErrorOr<Unit>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(p => p.Id == request.Id, cancellationToken);
        if (product is null)
        {
            return Error.Conflict("ProductNotFound","Ürün bulunamadı!");
        }

        product.IsHidden = true;
        product.DeletedDate = DateTime.Now;
        _mapper.Map(request, product);

        _productRepository.Update(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
