using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OngProject.Core.Helper.S3;

  

namespace OngProject.Core.Helper.CustomValidationsAttributes
{
    public class Base64Attribute : ValidationAttribute
    {

        public string GetErrorMessage() =>
            $"Formato base64 inválido.";

        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            bool isBase64Code = IsBase64String((string) value);

            if (!isBase64Code)
            {
                return new ValidationResult(GetErrorMessage());
            }
            return ValidationResult.Success;
        }

        private bool IsBase64String(string base64)
        {
            Base64ImageInspector.Base64ImageInspector.SplitIntoTypeAndImageData(base64, out string contentType, out string imageType, out string base64ImageData);

            Span<byte> buffer = new Span<byte>(new byte[base64ImageData.Length]);
            
            return Convert.TryFromBase64String(base64ImageData, buffer , out int bytesParsed);
        }
    }
}