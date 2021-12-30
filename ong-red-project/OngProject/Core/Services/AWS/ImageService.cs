using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OngProject.Common;
using OngProject.Core.Helper.Common;
using OngProject.Core.Helper.S3;
using OngProject.Core.Interfaces.IServices.AWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.Services.AWS
{
    public class ImageService : IImageService
    {
        private S3AwsHelper _s3AwsHelper;
        public ImageService(IOptions<AWSSettings> aWSSettings)
        {
            _s3AwsHelper = new S3AwsHelper( aWSSettings);
        }
        public async Task<bool> Delete(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }
            else
            {
                string nameImage = _s3AwsHelper.GetKeyFromUrl(name);
                AwsManagerResponse responseAws = await _s3AwsHelper.AwsFileDelete(nameImage);
                if (!String.IsNullOrEmpty(responseAws.Errors))
                    return false;
                return true;
            }
        }

        public async Task<Result> Save(string fileName, IFormFile image)
        {
            AwsManagerResponse responseAws;
            if (image != null)
            {
                if (ValidateFiles.ValidateImage(image))
                {
                    responseAws = await _s3AwsHelper.AwsUploadFile(fileName, image);
                    if (!String.IsNullOrEmpty(responseAws.Errors))
                    {
                        return new Result().Fail("Error al guardar imagen. Detalle:" + responseAws.Errors);
                    }
                    return new Result().Success(responseAws.Url);
                }
                else
                    return new Result().Fail("Extensión de imagen no válida. Debe ser jpg, png o jpeg.");
            }
            else
                return new Result().NotFound();
        }

        public async Task<string> SaveImageAsync(string fileName, IFormFile image)
        {
            AwsManagerResponse responseAws;
            if (image != null)
            {
                if (ValidateFiles.ValidateImage(image))
                {
                    responseAws = await _s3AwsHelper.AwsUploadFile(fileName, image);
                    if (!String.IsNullOrEmpty(responseAws.Errors))
                    {
                        return string.Empty;
                    }
                    return responseAws.Url;
                }
                else
                    return string.Empty;
            }
            else
                return string.Empty;
        }
    }
}
