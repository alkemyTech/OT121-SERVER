using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using OngProject.Core.Helper.S3;
using OngProject.Core.Interfaces.IServices.AWS;
using System;
using System.Threading.Tasks;

namespace OngProject.Core.Services.AWS
{
    public class ImageService : IImageService
    {
        #region Object and Constructor
        private IAmazonS3 _amazonS3;
        public ImageService(IAmazonS3 amazonS3)
        {
            _amazonS3 = amazonS3;
        }
        #endregion
        public async Task<string> SaveImageAsync(IFormFile file)
        {

            try
            {
                var putRequest = new PutObjectRequest()
                {
                    BucketName = "", // Bucket pendiente por definir
                    Key = file.FileName,
                    InputStream = file.OpenReadStream(),
                    ContentType = file.ContentType
                };
                await _amazonS3.PutObjectAsync(putRequest);

                return "Carga realizada correctamente";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        public string GetImageUrl(string imageName)
        {
            try
            {
                GetPreSignedUrlRequest request = new GetPreSignedUrlRequest
                {
                    BucketName = "",
                    Key = imageName,
                    Expires = DateTime.Now.AddDays(30)
                };
                string path = _amazonS3.GetPreSignedURL(request);

                return path;
            }catch(Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
