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
        private readonly IMailService _mailService;

        public ContactsController(IContactsServices contactsServices, IMailService mailService)
        {
            _contactsServices = contactsServices;
            _mailService = mailService;
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

        #region Documentation

        /// <summary>
        /// Endpoint para registrar contacto desde el sitio y envia un correo.
        /// </summary>
        /// <response code="200">Tarea ejecutada con exito, envia email cuando se registre un nuevo contacto en el sitio .</response>
        /// <response code="400">Errores de validacion.</response>

        #endregion Documentation

        [HttpPost()]
        public async Task<IActionResult> ContactsAsync(ContactDTO contact)
        {
            var registered = await _contactsServices.RegisterAsync(contact);
            await _mailService.SendEmailRegisteredContact(registered.Email, registered.Name);
            return Ok(registered);
        }

        #region Documentacion
        /// <summary>
        /// Endpoint para obtener un listado de todos los Usuarios
        /// </summary>
        /// <response code="200">Solicitud concretada con exito</response>
        /// <response code="401">Credenciales no validas</response> 
        #endregion
        [HttpGet()]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _contactsServices.GetAll());
        }

    }
}
