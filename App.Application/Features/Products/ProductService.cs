﻿using App.Application;
using App.Application.Contracts.Caching;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.ServiceBus;
using App.Application.Features.Products;
using App.Application.Features.Products.Create;
using App.Application.Features.Products.Dto;
using App.Application.Features.Products.Update;
using App.Application.Features.Products.UpdateStock;
using App.Domain.Entities;
using App.Domain.Events;
using AutoMapper;
using FluentValidation;

namespace App.Services.Products
{
    public class ProductService(IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        IValidator<CreateProductRequest> createProductRequestValidator,
        IMapper mapper, ICacheService cacheService, IServiceBus busService) : IProductService

    {

        private const string ProductListCacheKey = "ProductListCacheKey";

        public async Task<ServiceResult<List<ProductDto>>> GetTopPriceProductsAsync(int count)
        {
            var products = await productRepository.GetTopPriceProductAsync(count);

            //var productAsDto = products.Select(p => new ProductDto(p.Id, p.Name, p.Price, p.Stock)).ToList();

            var productAsDto = mapper.Map<List<ProductDto>>(products);

            return new ServiceResult<List<ProductDto>>()
            {
                Data = productAsDto
            };
        }

        public async Task<ServiceResult<List<ProductDto>>> GetAllListAsync()
        {
            var productListAsCached = await cacheService.GetAsync<List<ProductDto>>(ProductListCacheKey);

            if (productListAsCached is not null) return ServiceResult<List<ProductDto>>.Success(productListAsCached);
      

            var products = await productRepository.GetAllAsync();
            var productsAsDto = mapper.Map<List<ProductDto>>(products);

            await cacheService.AddAsync(ProductListCacheKey, productsAsDto, TimeSpan.FromMinutes(1));
            return ServiceResult<List<ProductDto>>.Success(productsAsDto);
        }

        public async Task<ServiceResult<List<ProductDto>>> GetPagedAllListAsync(int pageNumber, int pageSize)
        {
            var products = await productRepository.GetAllPagedAsync(pageNumber, pageSize);
            //var productsAsDto = products.Select(p => new ProductDto(p.Id, p.Name, p.Price, p.Stock)).ToList();
            var productsAsDto = mapper.Map<List<ProductDto>>(products);
            return ServiceResult<List<ProductDto>>.Success(productsAsDto);
        }

        public async Task<ServiceResult<ProductDto?>> GetByIdAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);

            if (product is null)
            {
                return ServiceResult<ProductDto?>.Fail("Product not found", System.Net.HttpStatusCode.NotFound);
            }

            //var productAsDto = new ProductDto(product!.Id, product.Name, product.Price, product.Stock);

            var productAsDto = mapper.Map<ProductDto>(product);

            return ServiceResult<ProductDto>.Success(productAsDto)!;
        }

        public async Task<ServiceResult<CreateProductResponse>> CreateAsync(CreateProductRequest request)
        {
            //throw new CriticalException("Kritik seviye bir hata meydana geldi");


            var anyProduct = await productRepository.AnyAsync(x => x.Name == request.Name);

            if (anyProduct)
            {
                return ServiceResult<CreateProductResponse>.Fail("Ürün ismi veritabanında bulunamadı", System.Net.HttpStatusCode.BadRequest);
            }


            var product = mapper.Map<Product>(request);

            await productRepository.AddAsync(product);
            await unitOfWork.SaveChangesAsync();

            await busService.PublishAsync(new ProductAddedEvent(product.Id, product.Name, product.Price));

            return ServiceResult<CreateProductResponse>.SuccessAsCreated(new CreateProductResponse(product.Id), $"api/products/{product.Id}");
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateProductRequest request)
        {
            // Fast fail
            // Guard clauses

            var isProductNameExist = await productRepository.AnyAsync(x => x.Name == request.Name && x.Id != id);

            if (isProductNameExist)
            {
                return ServiceResult.Fail("Ürün ismi veritabanında bulunmaktadır", System.Net.HttpStatusCode.BadRequest);
            }

            var product = mapper.Map<Product>(request);

            productRepository.Update(product);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(System.Net.HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> UpdateStockAsync(UpdateProductStockRequest request)
        {
            var product = await productRepository.GetByIdAsync(request.ProductId);

            if (product is null)
            {
                return ServiceResult.Fail("Product not found", System.Net.HttpStatusCode.NotFound);
            }

            product.Stock = request.Quantity;

            productRepository.Update(product);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(System.Net.HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);

            productRepository.Delete(product);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult.Success(System.Net.HttpStatusCode.NoContent);
        }
    }
}
