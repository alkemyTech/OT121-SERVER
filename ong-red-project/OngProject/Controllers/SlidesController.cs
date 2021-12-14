using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using OngProject.Core.Interfaces.IServices;
using OngProject.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SlidesController : ControllerBase
    {
        #region Object and Contructor
        private readonly ISlidesServices _slidesServices;

        public SlidesController(ISlidesServices slidesServices)
        {
            _slidesServices = slidesServices;
        }
        #endregion
        
    }
}