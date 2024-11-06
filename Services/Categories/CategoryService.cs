using App.Repositories;
using App.Repositories.Categories;
using App.Repositories.Products;
using App.Services.Categories.Create;
using App.Services.Categories.Dto;
using App.Services.Categories.Update;
using App.Services.Products.Create;
using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Collections.Generic;

namespace App.Services.Categories
{
    public class CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, IMapper mapper) : ICategoryService
    {

        public async Task<ServiceResult<CategoryWithProductsDto>> GetCategoryWithProductsAsync(int categoryId)
        {
            var category = await categoryRepository.GetCategoryWithProductAsync(categoryId);

            if (category is null)
            {
                return ServiceResult<CategoryWithProductsDto>.Fail("Kategory bulunmadı", System.Net.HttpStatusCode.NotFound);
            }

            var categoryAsDto = mapper.Map<CategoryWithProductsDto>(category);
            return ServiceResult<CategoryWithProductsDto>.Success(categoryAsDto);
        }

        public async Task<ServiceResult<List<CategoryWithProductsDto>>> GetCategoryWithProductsAsync()
        {
            var category = await categoryRepository.GetCategoryWithProducts().ToListAsync();

            var categoryAsDto = mapper.Map<List<CategoryWithProductsDto>>(category);
            return ServiceResult<List<CategoryWithProductsDto>>.Success(categoryAsDto);
        }

        public async Task<ServiceResult<List<CategoryDto>>> GetAllListAsync()
        {
            var categories = await categoryRepository.GetAll().ToListAsync();
            var categoriesDto = mapper.Map<List<CategoryDto>>(categories);
            return ServiceResult<List<CategoryDto>>.Success(categoriesDto);
        }

        public async Task<ServiceResult<CategoryDto>> GetByIdAsync(int Id)
        {
            var category = await categoryRepository.GetByIdAsync(Id);
            if (category is null)
            {
                return ServiceResult<CategoryDto>.Fail("Kategori Bulunmadı", System.Net.HttpStatusCode.NotFound);
            }

            var categoryAsDto = mapper.Map<CategoryDto>(category);

            return ServiceResult<CategoryDto>.Success(categoryAsDto);
        }

        public async Task<ServiceResult<int>> CreateAsync(CreateCategoryRequest request)
        {
            var anyCategory = await categoryRepository.Where(x => x.Name == request.Name).AnyAsync();

            if (anyCategory)
            {
                return ServiceResult<int>.Fail("Kategori ismi veritabanında bulunamadı", System.Net.HttpStatusCode.BadRequest);
            }

            var newCategory = mapper.Map<Category>(request);

            await categoryRepository.AddAsync(newCategory);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult<int>.SuccessAsCreated(newCategory.Id, $"api/categories/{newCategory.Id}");
        }

        public async Task<ServiceResult> UpdateAsync(int Id, UpdateCategoryRequest request)
        {
            var isCategoryNameExist = await categoryRepository.Where(x => x.Name == request.Name && x.Id != Id).AnyAsync();


            if (isCategoryNameExist)
            {
                return ServiceResult.Fail("Kategori ismi veritabanında bulunmaktadır.", System.Net.HttpStatusCode.BadRequest);
            }

            var category = mapper.Map<Category>(request);
            category.Id = Id;

            categoryRepository.Update(category);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(System.Net.HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> DeleteAsync(int Id)
        {
            var category = await categoryRepository.GetByIdAsync(Id);

            categoryRepository.Delete(category);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(System.Net.HttpStatusCode.NoContent);
        }
    }
}
