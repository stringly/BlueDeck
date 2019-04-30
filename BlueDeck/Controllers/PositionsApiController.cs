using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlueDeck.Models;
using Microsoft.AspNetCore.Authorization;
using BlueDeck.Models.APIModels;
using BlueDeck.Models.Types;

namespace BlueDeck.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class PositionsApiController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public PositionsApiController(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }

        /// <summary>
        /// Gets a List of Position Names/Ids of all Positions
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /PositionsApi/GetAll
        ///     {
        ///         {
        ///             positionId : "1",
        ///             name : "Squad 1 OIC"
        ///         },
        ///         {
        ///             positionId : "2",
        ///             name : "Shift 1 Commander"            
        ///         }
        ///     }
        ///         
        /// Use this method to retrieve the Position Id of a position by the Position's name.
        /// </remarks>
        /// <returns>A list of Position Names/Ids of all current Positions</returns>
        /// <response code="200">Returns a JSON object containing all current Positions</response>
        [HttpGet("GetAll")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public IEnumerable<PositionListAPIListItem> GetPositions()
        {
            return unitOfWork.Positions
                .GetAllPositionSelectListItems()
                .ToList()
                .ConvertAll(x => new PositionListAPIListItem(x));
        }

        /// <summary>
        /// Searches for a Position whose name or callsign matches the provided search string.
        /// </summary>
        /// <remarks>
        /// Sample Request
        /// 
        ///     GET /PositionsApi/SearchPositions/District I
        ///     {
        ///         {
        ///             positionId : "1",
        ///             name : "District I Commander"
        ///         },
        ///         {
        ///             positionId : "2",
        ///             name : "District I, Shift I Commander"            
        ///         }
        ///     }
        /// </remarks>
        /// <param name="searchString">The search string.</param>
        /// <returns>A list of Positions that match the search string</returns>
        /// <response code="200">Returns a list of Positions that match the search string</response>
        /// <response code="400">No Positions match the search string</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [HttpGet("SearchPositions/{searchString}")]
        public async Task<IActionResult> SearchPositions([FromRoute] string searchString)
        {
            IEnumerable<PositionListAPIListItem> result = new List<PositionListAPIListItem>();
            if (!string.IsNullOrEmpty(searchString))
            {
                List<Position> positions = unitOfWork.Positions.GetAll().ToList();
                char[] arr = searchString.ToCharArray();
                arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c)
                                  || char.IsWhiteSpace(c)
                                  || c == '-')));
                string lowerString = new string(arr);
                lowerString = lowerString.ToLower();
                
                positions = positions
                    .Where(x => x.Name.ToLower().Contains(lowerString)
                    || (x.Callsign != null && x.Callsign.Contains(lowerString.ToUpper()))).ToList();
                if (positions.Count > 0)
                {
                    result = positions.ToList().ConvertAll(x => new PositionListAPIListItem(x));
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
                return NotFound(new { status = "Not Found", message = $"No members match {searchString}" });
            }
        }
        /// <summary>
        /// Gets detailed information for a specific Position by BlueDeck Id.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /PositionApi/GetPosition/1
        ///     {
        ///         "memberId": 1,
        ///         "firstName": "John",
        ///         "lastName": "Smith",
        ///         "pgpdId": "1234",
        ///         "email": "example@email.com",
        ///         "contactNumbers": [
        ///             {
        ///                 "type": "Cell (Personal)",
        ///                 "phoneNumber": "(123) 456-7890"
        ///             }
        ///         ],
        ///         "rank": {
        ///             "rankName": "Lieutenant",
        ///             "rankShort": "Lt.",
        ///             "payGrade": "L05",
        ///             "isSworn": true
        ///         },
        ///         "gender": {
        ///             "name": "Male",
        ///             "abbreviation": "M"
        ///         },
        ///         "race": {
        ///             "name": "White",
        ///             "abbreviation": "W"
        ///         },
        ///         "dutyStatus": {
        ///             "name": "Full Duty",
        ///             "abbreviation": "F",
        ///             "hasPolicePower": true
        ///         },
        ///         "position": {
        ///             "positionId": 13,
        ///             "name": "Assistant District Commander, District I",
        ///             "isManager": true,
        ///             "isUnique": true,
        ///             "callsign": null,
        ///             "component": {
        ///                 "componentId": 50,
        ///                 "name": "District I",
        ///                 "acronym": "NULL",
        ///                 "parentComponent": null
        ///             }
        ///         },
        ///         "supervisor": {
        ///             "memberId": 1234,
        ///             "firstName": "Judy",
        ///             "lastName": "Johnson",
        ///             "pgpdId": "4321",
        ///             "email": "example2@email.net",
        ///             "contactNumbers": [
        ///                 {
        ///                     "type": "Cell (Work)",
        ///                     "phoneNumber": "(123) 456-7890"
        ///                 }
        ///             ],
        ///             "rank": {
        ///                 "rankName": "Captain",
        ///                 "rankShort": "Capt.",
        ///                 "payGrade": "L06",
        ///                 "isSworn": true
        ///             },
        ///             "gender": {
        ///                 "name": "Female",
        ///                 "abbreviation": "F"
        ///             },
        ///             "race": {
        ///                 "name": "White",
        ///                 "abbreviation": "W"
        ///             },
        ///             "dutyStatus": {
        ///                 "name": "Full Duty",
        ///                 "abbreviation": "F",
        ///                 "hasPolicePower": true
        ///             },
        ///             "position": {
        ///                 "positionId": 12,
        ///                 "name": "Commander, District I",
        ///                 "isManager": true,
        ///                 "isUnique": true,
        ///                 "callsign": "CAR-11",
        ///                 "component": {
        ///                     "componentId": null,
        ///                     "name": "",
        ///                     "acronym": "",
        ///                     "parentComponent": null
        ///                 }
        ///             },
        ///             "supervisor": null
        ///         }
        ///     } 
        ///     
        /// </remarks>
        /// <param name="id">The Position Id of the Position.</param>
        /// <returns>A collection of detailed information for the Position with the provided PositionId</returns>
        /// <response code="200">Detailed information for the Positionwith the provided PositionId</response>
        /// <response code="400">No Position with the provided Position Id was found</response>
        [HttpGet("GetPosition/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPosition([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!PositionExists(id))
            {
                return NotFound(new { status = "Not Found", message = $"No Position with id={id} exists." });
            }
            else
            {
                PositionApiResult result = await unitOfWork.Positions.GetApiPosition(id);
                return Ok(result);
            }

        }

        private bool PositionExists(int id)
        {
            return unitOfWork.Positions.Get(id) != null ? true : false;
        }

    }
}