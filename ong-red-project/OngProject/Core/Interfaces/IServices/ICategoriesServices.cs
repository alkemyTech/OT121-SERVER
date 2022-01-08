using OngProject.Common;
using OngProject.Core.DTOs.CategoriesDTOs;
using OngProject.Core.Entities;
using OngProject.Core.Helper.Pagination;
using System;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces.IServices
{
    public interface ICategoriesServices
    {
        Task<Result> Delete(int id);
        Task<PaginationDTO<string>> GetByPagingAsync(int page, int quantity);
        Task<CategoryGetDTO> Get(int id);
        Task<bool> ExistsByName(CategoryInsertDTO category);
        Task<Category> FindById(Int32 id);
        Task<CategoryGetDTO> Insert(CategoryInsertDTO category);
        Task<Category> Update(CategoryUpdateDTO category, Int32 id);
    }
}
