using App.Services.Categories.Create;
using App.Services.Categories.Dto;
using App.Services.Categories.Update;

namespace App.Services.Categories
{
    public interface ICategoryService
    {
        Task<ServiceResult<CategoryWithProductsDto>> GetCategoryWithProductsAsync(int categoryId);
        Task<ServiceResult<List<CategoryWithProductsDto>>> GetCategoryWithProductsAsync();
        Task<ServiceResult<List<CategoryDto>>> GetAllListAsync();
        Task<ServiceResult<CategoryDto>> GetByIdAsync(int Id);
        Task<ServiceResult<int>> CreateAsync(CreateCategoryRequest requst);
        Task<ServiceResult> UpdateAsync(int Id,UpdateCategoryRequest request);
        Task<ServiceResult> DeleteAsync(int Id);

    }
}
