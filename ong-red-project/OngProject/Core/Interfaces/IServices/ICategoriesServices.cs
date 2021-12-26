using OngProject.Common;
using OngProject.Core.DTOs.CategoriesDTOs;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces.IServices
{
    public interface ICategoriesServices
    {
        Task<Result> Delete(int id);
        Task<string[]> GetCategories();
        Task<CategoryGetDTO> Get(int id);
        Task<bool> ExistsByName(CategoryInsertDTO category);
        Task<CategoryGetDTO> Insert(CategoryInsertDTO category);
    }
}
