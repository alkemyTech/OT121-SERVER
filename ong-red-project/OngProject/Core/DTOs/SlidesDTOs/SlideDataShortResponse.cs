using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.DTOs.SlidesDTOs
{
    public class SlideDataShortResponse
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        public string Text { get; set; }

        public int Order { get; set; }

    }
}