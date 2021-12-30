using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.DTOs.SlidesDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces.IServices
{
    public interface ISlidesServices
    {
        bool EntityExist(int id);
        Task<List<SlideDataShortResponse> > GetListOfSlides();
        Task<SlideDataFullResponse> GetSlideById(int id);
    }

}


