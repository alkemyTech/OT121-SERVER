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
        private readonly IConfiguration _configuration;
        private IAmazonS3 _amazonS3;
        public ImageService(IAmazonS3 amazonS3, IConfiguration configuration)
        {
            _amazonS3 = amazonS3;
            _configuration = configuration;
        }
        #endregion
        public async Task<string> SaveImageAsync(IFormFile file)
        {
            AmazonS3Config config = new AmazonS3Config
            {
                SignatureVersion = "4",
                RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName("us-east-2") // Región pendiente por definir
        };

            try
            {
                _amazonS3 = new AmazonS3Client(_configuration["aws_access_key_id"], _configuration["aws_secret_access_key"], config);
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
    }
}
