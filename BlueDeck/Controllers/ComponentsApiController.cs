using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlueDeck.Models;
using BlueDeck.Models.APIModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlueDeck.Controllers
{
    /// <summary>
    /// Controller that handles Web API requests for the <see cref="Component"/> entity.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ComponentsApiController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentsApiController"/> class.
        /// </summary>
        /// <param name="unit">The injected <see cref="IUnitOfWork"/> obtained from the services middleware.</param>
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
        ///             componentId : "1"
        ///         },
        ///         {
        ///             name : "Automotive Services",
        ///             componentId : "2"
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

        /// <summary>
        /// Searches for a Component whose Name or Acronym matches the provided search string.
        /// </summary>
        /// <remarks>
        /// Sample Request
        /// 
        ///     GET /ComponentApi/SearchComponents/Example
        ///     {
        ///         {
        ///             name : "Example District",
        ///             componentId : "1"
        ///         },
        ///         {
        ///             name : "Example Office of Administration",
        ///             componentId : "4"
        ///         }
        ///     }
        /// </remarks>
        /// <param name="searchString">The search string.</param>
        /// <returns>A list of Components that match the search string</returns>
        /// <response code="200">Returns a list of Components that match the search string</response>
        /// <response code="400">No Components match the search string</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [HttpGet("SearchComponents/{searchString}")]
        public async Task<IActionResult> SearchComponents([FromRoute] string searchString)
        {
            try
            {
                IEnumerable<ComponentListApiListItem> result = new List<ComponentListApiListItem>();
                if (!string.IsNullOrEmpty(searchString))
                {
                    IEnumerable<Component> components = unitOfWork.Components.GetAll();
                    char[] arr = searchString.ToCharArray();
                    arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c)
                                      || char.IsWhiteSpace(c)
                                      || c == '-')));
                    string lowerString = new string(arr);
                    lowerString = lowerString.ToLower();
                    components = components
                        .Where(x => x.Name.ToLower().Contains(lowerString)
                        || x.Acronym.ToLower().Contains(lowerString));
                    if(components.Count() > 0)
                    {
                        result = components.ToList().ConvertAll(x => new ComponentListApiListItem(x));
                    }
                
                }
                else
                {
                    return NotFound(new { status = "Not Found", message = "Search string parameter is required." });
                }
                if (result.Count() > 0)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(new { status = "Not Found", message = $"No Components match {searchString}" });
                }

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "The service encountered an error." });
            }
        }
        /// <summary>
        /// Gets detailed information for a specific Component by Id.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /ComponentApi/GetComponent/1
        ///     
        /// </remarks>
        /// <param name="id">The Component Id of the Component.</param>
        /// <returns>A collection of detailed information for the Component with the provided Component Id</returns>
        /// <response code="200">Detailed information for the Component with the provided Component Id</response>
        /// <response code="400">No Component with the provided Component Id was found</response>
        [HttpGet("GetComponent/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetComponent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (!ComponentExists(id))
                {
                    return NotFound(new { status = "Not Found", message = $"No Component with id {id} could be found" });
                }
                else
                {
                    ComponentApiResult result = await unitOfWork.Components.GetApiComponent(id);
                    return Ok(result);
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "The service encountered an error." });
            }
        }

        private bool ComponentExists(int id)
        {
            return unitOfWork.Components.Get(id) != null ? true : false;
        }
    }
}