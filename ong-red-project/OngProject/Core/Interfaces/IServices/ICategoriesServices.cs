using OngProject.Common;
using OngProject.Core.DTOs.CategoriesDTOs;
using OngProject.Core.Entities;
using System;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces.IServices
{
    public interface ICategoriesServices
    {
        Task<Result> Delete(int id);
        Task<string[]> GetCategories();
        Task<CategoryGetDTO> Get(int id);
        Task<bool> ExistsByName(CategoryInsertDTO category);
        Task<Category> FindById(Int32 id);
        Task<CategoryGetDTO> Insert(CategoryInsertDTO category);
        Task<Category> Update(CategoryUpdateDTO category, Int32 id);
    }
}
