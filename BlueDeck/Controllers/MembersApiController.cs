using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlueDeck.Models;
using Microsoft.AspNetCore.Authorization;
using BlueDeck.Models.APIModels;

namespace BlueDeck.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class MembersApiController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public MembersApiController(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }

        /// <summary>
        /// Gets a List of Member Names/BlueDeck Ids of all Members
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /MembersApi/GetAll
        ///     {
        ///         {
        ///             name : "Bob Jones",
        ///             blueDeckId : "1"
        ///         },
        ///         {
        ///             name : "Steve Johnson",
        ///             blueDeckId : "2"
        ///         }
        ///     }
        ///         
        /// Use this method to retrieve the BlueDeck Id of a member by the Member's name.
        /// </remarks>
        /// <returns>A list of Member Names/BlueDeck Ids of all current Members</returns>
        /// <response code="200">Returns a JSON object containing all current Members</response>
        [HttpGet("GetAll")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetMembers()
        {
            try
            {
                return Ok(unitOfWork.Members
                .GetAllMemberSelectListItems()
                .ToList()
                .ConvertAll(x => new MemberListAPIListItem(x)));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "The service encountered an error." });
            }
            
        }

        /// <summary>
        /// Searches for a member whose First Name, Last Name, or PGPD ID# matches the provided search string.
        /// </summary>
        /// <remarks>
        /// Sample Request
        /// 
        ///     GET /MembersApi/SearchMembers/smith
        ///     {
        ///         {
        ///             name : "Bob Smith",
        ///             blueDeckId : "1"
        ///         },
        ///         {
        ///             name : "John Smith",
        ///             blueDeckId : "4"
        ///         }
        ///     }
        /// </remarks>
        /// <param name="searchString">The search string.</param>
        /// <returns>A list of Members that match the search string</returns>
        /// <response code="200">Returns a list of Members that match the search string</response>
        /// <response code="400">No members match the search string</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [HttpGet("SearchMembers/{searchString}" )]
        public async Task<IActionResult> SearchMembers([FromRoute] string searchString)
        {
            try {
                IEnumerable<MemberListAPIListItem> result = new List<MemberListAPIListItem>();
                if (!string.IsNullOrEmpty(searchString))
                {
                    IEnumerable<Member> members = unitOfWork.Members.GetMembersWithRank();
                    char[] arr = searchString.ToCharArray();
                    arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c)
                                      || char.IsWhiteSpace(c)
                                      || c == '-')));
                    string lowerString = new string(arr);
                    lowerString = lowerString.ToLower();
                    members = members
                        .Where(x => x.LastName.ToLower().Contains(lowerString)
                        || x.FirstName.ToLower().Contains(lowerString)                     
                        || x.IdNumber.Contains(lowerString));
                    if(members.Count() > 0)
                    {
                        result = members.ToList().ConvertAll(x => new MemberListAPIListItem(x));
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
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "The service encountered an error." });
            }
        }

        /// <summary>
        /// Gets detailed information for a specific member by BlueDeck Id.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /MembersApi/GetMember/1
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
        /// <param name="id">The BlueDeck Id of the member.</param>
        /// <returns>A collection of detailed information for the Member with the provided BlueDeckId</returns>
        /// <response code="200">Detailed information for the Member with the provided BlueDeckId</response>
        /// <response code="400">No member with the provided BlueDeck Id was found</response>
        [HttpGet("GetMember/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetMember([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {

                if (!MemberExists(id))
                {
                    return NotFound(new { status = "Not Found", message = $"No Member with id={id} exists." });
                }
                else
                {
                    MemberApiResult result = await unitOfWork.Members.GetApiMember(id);            
                    return Ok(result);
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "The service encountered an error." });
            }
        }
             

        private bool MemberExists(int id)
        {
            return unitOfWork.Members.Get(id) != null ? true : false;
        }
    }
}