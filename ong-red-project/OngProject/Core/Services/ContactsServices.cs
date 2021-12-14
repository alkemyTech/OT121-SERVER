using Microsoft.Extensions.Configuration;
using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.Helper.Pagination;
using OngProject.Core.Interfaces.IServices;
using OngProject.Core.Mapper;
using OngProject.Infrastructure.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.Services
{
    public class ContactsServices : IContactsServices
    {
        #region Objects and Constructor
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUriService uriService;
        public ContactsServices(IUnitOfWork unitOfWork, IUriService uriService)
        {
            this._unitOfWork = unitOfWork;
            this.uriService = uriService;
        }
        #endregion
       
        public async Task<ContactDTO> GetById(int id)
        {

            var mapper = new EntityMapper();
            var contact = await _unitOfWork.ContactsRepository.GetById(id);
            var contactDTO = mapper.FromContactsToContactsDto(contact);
            return contactDTO;
        }
        public bool EntityExists(int id)
        {
            return _unitOfWork.ContactsRepository.EntityExists(id);
        }
        
    }
}
