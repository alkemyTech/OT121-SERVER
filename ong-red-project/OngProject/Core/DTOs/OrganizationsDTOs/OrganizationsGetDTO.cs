using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.DTOs
{
    public class OrganizationsGetDTO
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Address { get; set; }
        public int Phone { get; set; }
    }
}
