using OngProject.Common;
using OngProject.Core.DTOs.TestimonialsDTOs;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces.IServices
{
    public interface ITestimonialsServices
    {
        Task<Result> CreateAsync(TestimonialsCreateDTO testimonialsCreate);
    }
}