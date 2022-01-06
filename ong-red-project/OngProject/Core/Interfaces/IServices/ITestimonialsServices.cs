using OngProject.Common;
using OngProject.Core.DTOs.TestimonialsDTOs;
using OngProject.Core.Helper.Pagination;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces.IServices
{
    public interface ITestimonialsServices
    {
        Task<Result> CreateAsync(TestimonialsCreateDTO testimonialsCreate);

        Task<Result> UpdateAsync(TestimonialsUpdateDTO testimonialsUpdate);

        Task<PaginationDTO<TestimonialsDTO>> GetByPagingAsync(int page, int quantity);
    }
}