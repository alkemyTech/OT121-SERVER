using OngProject.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces.IServices
{
    public interface ICategoriesServices
    {
        Task<Result> Delete(int id);
        Task<string[]> GetCategories(int page);
    }
}
