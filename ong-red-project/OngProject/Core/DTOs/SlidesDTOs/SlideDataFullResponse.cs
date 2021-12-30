using System;

namespace OngProject.Core.DTOs.SlidesDTOs
{
    public class SlideDataFullResponse : SlideDataShortResponse
    {
        public DateTime CreatedAt { get; set; }
        public int OrganizationId { get; set; }        
    }
}


 