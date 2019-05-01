using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlueDeck.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlueDeck.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ComponentsApiController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public ComponentsApiController(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }
        /// <summary>
        /// Gets a List of Component Names/Ids of all Components
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /ComponentApi/GetAll
        ///     {
        ///         {
        ///             name : "District I",
        ///             blueDeckId : "1"
        ///         },
        ///         {
        ///             name : "Automotive Services",
        ///             blueDeckId : "2"
        ///         }
        ///     }
        ///         
        /// Use this method to retrieve the Component Id of a Component by the Component's name.
        /// </remarks>
        /// <returns>A list of Component Names/Ids of all current Component</returns>
        /// <response code="200">Returns a JSON object containing all current Component</response>
        [HttpGet("GetAll")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetComponents()
        {
            try
            {
                return Ok(unitOfWork.Components
                    .GetComponentSelectListItems()
                    .ToList()
                    .ConvertAll(x => new ComponentListApiListItem(x)));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "The service encountered an error." });
            }
        }
    }
}