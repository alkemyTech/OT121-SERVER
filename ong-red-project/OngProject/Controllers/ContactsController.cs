using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.Helper.Pagination;
using OngProject.Core.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        #region Objects and Constructor
        private readonly IContactsServices _contactsServices;
        public ContactsController(IContactsServices contactsServices)
        {
            _contactsServices = contactsServices;
        }
        #endregion

       

        #region Documentacion
        /// <summary>
        /// Endpoint para obtener un contact por id. Se debe ser ADMINISTRADOR
        /// </summary>
        /// <response code="200">Solicitud concretada con exito</response>
        /// <response code="401">Credenciales no validas</response> 
        #endregion
        
        [HttpGet("{id}")]
        public async Task<ActionResult<ContactDTO>> Get(int id)
        {
            try
            {
                var test= _contactsServices.EntityExists(id);
                if (test==false)
                    return NotFound();

                var contact = await _contactsServices.GetById(id);
                return Ok(contact);
            }
            catch (Exception result)
            {
                return BadRequest(result.Message);
            }
        }
      
    }
}
