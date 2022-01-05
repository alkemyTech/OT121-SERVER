using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.Helper.Base64ImageInspector
{
    public class Base64ImageInspector
    {
        public static void SplitIntoTypeAndImageData(string encodedImage, out string contentType ,out string imageType, out string content)
        {
            contentType = "";
            imageType = "";
            content = encodedImage;
            if(!string.IsNullOrEmpty(encodedImage) && encodedImage.StartsWith("data:") )
            {
                int indexOfFirstComma = encodedImage.IndexOf(',');
                if(indexOfFirstComma + 1 < encodedImage.Length || indexOfFirstComma >= 0)
                {
                    GetTypeInfo(encodedImage, indexOfFirstComma, out contentType, out imageType);
                    content = encodedImage.Substring(indexOfFirstComma + 1); 
                }
            }
        }

        private static void GetTypeInfo(string encodedImage, int indexOfFirstComma, out string contentType, out string imageType)
        {
            string header = encodedImage.Substring(0,indexOfFirstComma);
            if(header.Contains("image/jpg"))
            {
                contentType = "image/jpg";
                imageType = "jpg";
            }else if(header.Contains("image/png"))
            {
                contentType = "image/png";
                imageType = "png";
            }else if(header.Contains("image/jpeg"))
            {
                contentType = "image/jpeg";
                imageType = "jpeg";
            }
            else    
            {
                contentType = "";
                imageType = "";
            }
        }
    }
}