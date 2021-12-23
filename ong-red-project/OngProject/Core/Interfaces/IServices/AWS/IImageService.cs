using Microsoft.AspNetCore.Http;
using OngProject.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces.IServices.AWS
{
    public interface IImageService
    {
        Task<Result> Save(string fileName, IFormFile image);
        Task<bool> Delete(string name);
    }
}
