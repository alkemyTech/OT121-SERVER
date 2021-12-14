using System;
using Microsoft.AspNetCore.Http;

namespace OngProject.Core.DTOs
{
    public class OrganizationUpdateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IFormFile Image { get; set; }
        public string Address { get; set; }
        public int Phone { get; set; }
        public string Email { get; set; }
        public string WelcomeText { get; set; }
        public string AboutUsText { get; set; }
        
    }
}
