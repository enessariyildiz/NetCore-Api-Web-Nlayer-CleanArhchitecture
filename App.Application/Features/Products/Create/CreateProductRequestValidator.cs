﻿using App.Application.Contracts.Persistence;
using FluentValidation;

namespace App.Application.Features.Products.Create
{
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        private readonly IProductRepository _repository;

        public CreateProductRequestValidator(IProductRepository repository)
        {
            _repository = repository;
        }

        public CreateProductRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ürün ismi gereklidir")
                .Length(3, 10).WithMessage("Ürün ismi 3 ile 10 karakter arasında olmalıdır");
            //.Must(MustUniqeProductName).WithMessage("Ürün ismi veritabanında bulunmaktadır");

            // Price Validation
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Ürün fiyatı 0'dan büyük olmalıdır");

            // Price Validation
            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Ürün kategori değeri 0'dan büyük olmalıdır");



            // Stock InclusiveBetween Validation
            RuleFor(x => x.Stock)
                .InclusiveBetween(1, 100).WithMessage("Stok adedi 1 ile 100 arasında olmalıdır");
        }

        /*
        private bool MustUniqeProductName(string name)
        {
            return !_repository.Where(x => x.Name == name).Any();
        }
        */
    }
}
