using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OngProject.Common;
using OngProject.Core.DTOs;

namespace OngProject.Core.Interfaces.IServices
{
    public interface IOrganizationsServices
    {
        Task<OrganizationsGetDTO> GetById(int id);
        bool EntityExists(int id);
    }
}
