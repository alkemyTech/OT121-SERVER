using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.Interfaces.IServices;
using OngProject.Core.Interfaces.IServices.AWS;
using OngProject.Core.Mapper;
using OngProject.Core.Services.AWS;
using OngProject.Infrastructure.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.Services
{
    public class OrganizationsServices : IOrganizationsServices
    {
        #region Object and Constructor
        private readonly IUnitOfWork _unitOfWork;
        private readonly EntityMapper _mapper;
        public OrganizationsServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = new EntityMapper();
        }
        #endregion

        public bool EntityExists(int id)
        {
            return _unitOfWork.OrganizationsRepository.EntityExists(id);
        }

        public async Task<OrganizationsGetDTO> GetById(int id)
        {
            var organizations = await _unitOfWork.OrganizationsRepository.GetById(id);
            var organizationsDTO = _mapper.FromOrganizationToOrganizationGetDto(organizations);
            return organizationsDTO;
        }
    }
}
