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
    /// <summary>
    /// API Controller that Retrieves BlueDeck Member information
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class MembersApiController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="MembersApiController"/> class.
        /// </summary>
        /// <param name="unit">The unit.</param>
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
        ///     GET /MembersApi/GetMemberByBlueDeckId/1
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
        [HttpGet("GetMemberByBlueDeckId/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetMemberByBlueDeckId([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {

                if (!MemberExists(id))
                {
                    return NotFound(new { status = "Not Found", message = $"No Member with id {id} could be found." });
                }
                else
                {
                    MemberApiResult result = await unitOfWork.Members.GetApiMemberByBlueDeckId(id);            
                    return Ok(result);
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "The service encountered an error." });
            }
        }

        /// <summary>
        /// Gets detailed information for a specific member by Organization Id.
        /// </summary>
        /// <remarks>
        /// Use this method to attempt to retrieve Member details using the Member's known Organization Id Number. The Organization Id is distinct from the Member's BlueDeck Id
        /// Sample request:
        /// 
        ///     GET /MembersApi/GetMemberByOrgId/1234
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
        /// <param name="id">The Organization Id of the member.</param>
        /// <returns>A collection of detailed information for the Member with the provided Organization Id</returns>
        /// <response code="200">Detailed information for the Member with the provided Organization Id</response>
        /// <response code="400">No member with the provided Organization Id was found</response>
        [HttpGet("GetMemberByOrgId/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetMemberByOrgId([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {

                if (!MemberExists(id))
                {
                    return NotFound(new { status = "Not Found", message = $"No Member with id {id} could be found." });
                }
                else
                {
                    MemberApiResult result = await unitOfWork.Members.GetApiMemberByOrgId(id);            
                    return Ok(result);
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "The service encountered an error." });
            }
        }

        /// <summary>
        /// Gets the first supervisor for the Member with the given BlueDeck Id.
        /// </summary>
        /// <remarks>
        /// This method will parse the current organizational hierarchy and retrieve the first supervisory employee in the chain of command above the member with the given BlueDeck Id.
        /// 
        ///     GET /MembersApi/GetSupervisorForBlueDeckId/1
        ///     {
        ///         "memberId": 1,
        ///         "firstName": "Bob",
        ///         "lastName": "Smith",
        ///         "pgpdId": "1234",
        ///         "email": "test@mail.com",
        ///         "contactNumbers": [],
        ///         "rank": {
        ///             "rankId": 5,
        ///             "rankName": "Lieutenant",
        ///             "rankShort": "Lt.",
        ///             "payGrade": "L05",
        ///             "isSworn": true
        ///         },
        ///         "gender": {
        ///             "genderId": 2,
        ///             "genderName": "Male",
        ///             "abbreviation": "M"
        ///         },
        ///         "race": {
        ///             "raceId": 2,
        ///             "raceName": "White",
        ///             "abbreviation": "W"
        ///         },
        ///         "dutyStatus": {
        ///             "name": "Full Duty",
        ///             "abbreviation": "F",
        ///             "hasPolicePower": true
        ///         },
        ///         "position": {
        ///             "positionId": 1,
        ///             "name": "Shift Commander",
        ///             "isManager": true,
        ///             "isUnique": true,
        ///             "callsign": "31-ADAM"
        ///         },
        ///         "supervisor": null
        ///     }
        ///     
        /// </remarks>
        /// <param name="id">The BlueDeck Id of the Member whose supervisor you are trying to retrieve.</param>
        /// <returns></returns>
        /// <response code="200">Detailed information for the supervisor of the Member with the provided BlueDeck Id</response>
        /// <response code="400">No member with the provided BlueDeck Id was found</response>
        [HttpGet("GetSupervisorForBlueDeckId/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetSupervisorForBlueDeckId([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (!MemberExists(id))
                {
                    return NotFound(new { status = "Not Found", message = $"No Member with id {id} could be found." });
                }
                else
                {
                    Member supervisor = await unitOfWork.Members.FindNearestManagerForMemberId(id);
                    if (supervisor != null)
                    {
                        return Ok(new MemberApiResult(supervisor));
                    }
                    else
                    {
                        return NotFound(new { status = "Not Found", message = $"No supervisor for Member {id} could be found." }); 
                    }
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "The service encountered an error."});
            }
        }

        /// <summary>
        /// Gets the first supervisor for the Member with the given BlueDeck Id.
        /// </summary>
        /// <remarks>
        /// This method will parse the current organizational hierarchy and retrieve the first supervisory employee in the chain of command above the member with the given BlueDeck Id.
        /// 
        ///     GET /MembersApi/GetSupervisorForOrgId/1
        ///     {
        ///         "memberId": 1,
        ///         "firstName": "Bob",
        ///         "lastName": "Smith",
        ///         "pgpdId": "1234",
        ///         "email": "test@mail.com",
        ///         "contactNumbers": [],
        ///         "rank": {
        ///             "rankId": 5,
        ///             "rankName": "Lieutenant",
        ///             "rankShort": "Lt.",
        ///             "payGrade": "L05",
        ///             "isSworn": true
        ///         },
        ///         "gender": {
        ///             "genderId": 2,
        ///             "genderName": "Male",
        ///             "abbreviation": "M"
        ///         },
        ///         "race": {
        ///             "raceId": 2,
        ///             "raceName": "White",
        ///             "abbreviation": "W"
        ///         },
        ///         "dutyStatus": {
        ///             "name": "Full Duty",
        ///             "abbreviation": "F",
        ///             "hasPolicePower": true
        ///         },
        ///         "position": {
        ///             "positionId": 1,
        ///             "name": "Shift Commander",
        ///             "isManager": true,
        ///             "isUnique": true,
        ///             "callsign": "31-ADAM"
        ///         },
        ///         "supervisor": null
        ///     }
        ///     
        /// </remarks>
        /// <param name="id">The Organization Id of the Member whose supervisor you are trying to retrieve.</param>
        /// <returns></returns>
        /// <response code="200">Detailed information for the supervisor of the Member with the provided Organization Id</response>
        /// <response code="400">No member with the provided Organization Id was found</response>
        [HttpGet("GetSupervisorForOrgId/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetSupervisorForOrgId([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (!MemberExists(id))
                {
                    return NotFound(new { status = "Not Found", message = $"No Member with id {id} could be found." });
                }
                else
                {
                    Member m = unitOfWork.Members.Find(x => x.IdNumber == id).First();
                    Member supervisor = await unitOfWork.Members.FindNearestManagerForMemberId(m.MemberId);
                    if (supervisor != null)
                    {
                        return Ok(new MemberApiResult(supervisor));
                    }
                    else
                    {
                        return NotFound(new { status = "Not Found", message = $"No supervisor for Member {id} could be found." }); 
                    }
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "The service encountered an error."});
            }
        }

        /// <summary>
        /// Gets the first assistant supervisor or supervisor for the Member with the given BlueDeck Id.
        /// </summary>
        /// <remarks>
        /// This method will parse the current organizational hierarchy and retrieve the first supervisory employee in the chain of command above the member with the given BlueDeck Id.
        /// Use this method if you want to retrieve the Member's "9-Car."
        /// 
        ///     GET /MembersApi/GetAssistantSupervisorOrSupervisorForBlueDeckId/1234
        ///     {
        ///         "memberId": 1,
        ///         "firstName": "Bob",
        ///         "lastName": "Smith",
        ///         "pgpdId": "1234",
        ///         "email": "test@mail.com",
        ///         "contactNumbers": [],
        ///         "rank": {
        ///             "rankId": 5,
        ///             "rankName": "Lieutenant",
        ///             "rankShort": "Lt.",
        ///             "payGrade": "L05",
        ///             "isSworn": true
        ///         },
        ///         "gender": {
        ///             "genderId": 2,
        ///             "genderName": "Male",
        ///             "abbreviation": "M"
        ///         },
        ///         "race": {
        ///             "raceId": 2,
        ///             "raceName": "White",
        ///             "abbreviation": "W"
        ///         },
        ///         "dutyStatus": {
        ///             "name": "Full Duty",
        ///             "abbreviation": "F",
        ///             "hasPolicePower": true
        ///         },
        ///         "position": {
        ///             "positionId": 1,
        ///             "name": "Shift Commander",
        ///             "isManager": true,
        ///             "isUnique": true,
        ///             "callsign": "31-ADAM"
        ///         },
        ///         "supervisor": null
        ///     }
        /// </remarks>
        /// <param name="id">The BlueDeck Id of the Member whose supervisor you are trying to retrieve.</param>
        /// <returns></returns>
        /// <response code="200">Detailed information for the supervisor of the Member with the provided BlueDeck Id</response>
        /// <response code="400">No member with the provided Organization Id was found</response>
        [HttpGet("GetAssistantSupervisorOrSupervisorForBlueDeckId/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAssistantSupervisorOrSupervisorForBlueDeckId([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (!MemberExists(id))
                {
                    return NotFound(new { status = "Not Found", message = $"No Member with id {id} could be found." });
                }
                else
                {
                    Member supervisor = await unitOfWork.Members.FindNearestAssistantManagerOrManagerForMemberId(id);
                    if (supervisor != null)
                    {
                        return Ok(new MemberApiResult(supervisor));
                    }
                    else
                    {
                        return NotFound(new { status = "Not Found", message = $"No supervisor for Member {id} could be found." }); 
                    }
                    
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "The service encountered an error."});
            }
        }

        /// <summary>
        /// Gets the first assistant supervisor or supervisor for the Member with the given Organization Id.
        /// </summary>
        /// <remarks>
        /// This method will parse the current organizational hierarchy and retrieve the first supervisory employee in the chain of command above the member with the given Organization Id.
        /// Use this method if you want to retrieve the Member's "9-Car."
        /// 
        ///     GET /MembersApi/GetAssistantSupervisorOrSupervisorForOrgId/1234
        ///     {
        ///         "memberId": 1,
        ///         "firstName": "Bob",
        ///         "lastName": "Smith",
        ///         "pgpdId": "1234",
        ///         "email": "test@mail.com",
        ///         "contactNumbers": [],
        ///         "rank": {
        ///             "rankId": 5,
        ///             "rankName": "Lieutenant",
        ///             "rankShort": "Lt.",
        ///             "payGrade": "L05",
        ///             "isSworn": true
        ///         },
        ///         "gender": {
        ///             "genderId": 2,
        ///             "genderName": "Male",
        ///             "abbreviation": "M"
        ///         },
        ///         "race": {
        ///             "raceId": 2,
        ///             "raceName": "White",
        ///             "abbreviation": "W"
        ///         },
        ///         "dutyStatus": {
        ///             "name": "Full Duty",
        ///             "abbreviation": "F",
        ///             "hasPolicePower": true
        ///         },
        ///         "position": {
        ///             "positionId": 1,
        ///             "name": "Shift Commander",
        ///             "isManager": true,
        ///             "isUnique": true,
        ///             "callsign": "31-ADAM"
        ///         },
        ///         "supervisor": null
        ///     }
        /// </remarks>
        /// <param name="id">The Organization Id of the Member whose supervisor you are trying to retrieve.</param>
        /// <returns></returns>
        /// <response code="200">Detailed information for the supervisor of the Member with the provided Organization Id</response>
        /// <response code="400">No member with the provided Organization Id was found</response>
        [HttpGet("GetAssistantSupervisorOrSupervisorForOrgId/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAssistantSupervisorOrSupervisorForOrgId([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (!MemberExists(id))
                {
                    return NotFound(new { status = "Not Found", message = $"No Member with id {id} could be found." });
                }
                else
                {
                    Member m = unitOfWork.Members.Find(x => x.IdNumber == id).First();
                    Member supervisor = await unitOfWork.Members.FindNearestAssistantManagerOrManagerForMemberId(m.MemberId);
                    if (supervisor != null)
                    {
                        return Ok(new MemberApiResult(supervisor));
                    }
                    else
                    {
                        return NotFound(new { status = "Not Found", message = $"No supervisor for Member {id} could be found." }); 
                    }
                    
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "The service encountered an error."});
            }
        }

        /// <summary>
        /// Gets a list of subordinates for a given Member by BlueDeck Id.
        /// </summary>
        /// <remarks>
        /// 
        ///     GET: /MembersApi/GetSubordinatesForBlueDeckId/1234
        ///     [
        ///         {
        ///             "name": "Lt. Bob Jones #1234",
        ///             "blueDeckId": 1,
        ///             "orgId": "1234"
        ///         },
        ///         {
        ///             "name": "P/O John Smith #1111",
        ///             "blueDeckId": 1410,
        ///             "orgId": "1111"
        ///         },
        ///         {
        ///             "name": "Sgt. John Doe #2222",
        ///             "blueDeckId": 1065,
        ///             "orgId": "2222"
        ///         }
        ///     ]
        /// Use this method to retrieve a basic list of Members who are subordinate to the Member with a given BlueDeck Id.
        /// </remarks>
        /// <param name="id">The BlueDeck Id of the Supervisor for whom you would like a subordinate list.</param>
        /// <returns>A JSON Object containing a list of all members subordinate to the Member with the given BlueDeck Id.</returns>
        /// <response code="200">A list of Members subordinate to the Member with the given BlueDeck Id.</response>
        /// <response code="400">No member with the provided Organization Id was found, or the Member has no subordinates.</response>
        [HttpGet("GetSubordinatesForBlueDeckId/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetSubordinatesForBlueDeckId([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (!MemberExists(id))
                {
                    return NotFound(new { status = "Not Found", message = $"No Member with id {id} could be found." });
                }
                else
                {
                    List<MemberListAPIListItem> result = await unitOfWork.Members.GetSubordinateMemberApiMemberForBlueDeckId(id);
                    if (result.Count == 0)
                    {
                        return NotFound(new {status = "Not Found", message = $"No subordinates found for member with id {id}."});
                    }
                    else
                    {
                        return Ok(result);
                    }                    
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "The service encountered an error." });
            }
        }

        /// <summary>
        /// Gets a list of subordinates for a given Member by Organization Id.
        /// </summary>
        /// <remarks>
        /// 
        ///     GET: /MembersApi/GetSubordinatesForOrgId/1234
        ///     [
        ///         {
        ///             "name": "Lt. Bob Jones #1234",
        ///             "blueDeckId": 1,
        ///             "orgId": "1234"
        ///         },
        ///         {
        ///             "name": "P/O John Smith #1111",
        ///             "blueDeckId": 1410,
        ///             "orgId": "1111"
        ///         },
        ///         {
        ///             "name": "Sgt. John Doe #2222",
        ///             "blueDeckId": 1065,
        ///             "orgId": "2222"
        ///         }
        ///     ]
        /// Use this method to retrieve a basic list of Members who are subordinate to the Member with a given Organization Id.
        /// </remarks>
        /// <param name="id">The Organization Id of the Supervisor for whom you would like a subordinate list.</param>
        /// <returns>A JSON Object containing a list of all members subordinate to the Member with the given Organization Id.</returns>
        /// <response code="200">A list of Members subordinate to the Member with the given Organization Id.</response>
        /// <response code="400">No member with the provided Organization Id was found, or the Member has no subordinates.</response>
        [HttpGet("GetSubordinatesForOrgId/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetSubordinatesForOrgId([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (!MemberExists(id))
                {
                    return NotFound(new { status = "Not Found", message = $"No Member with id {id} could be found." });
                }
                else
                {
                    Member member = unitOfWork.Members.Find(x => x.IdNumber == id).FirstOrDefault();                   
                    List<MemberListAPIListItem> result = await unitOfWork.Members.GetSubordinateMemberApiMemberForBlueDeckId(member.MemberId);
                    if (result.Count == 0)
                    {
                        return NotFound(new {status = "Not Found", message = $"No subordinates found for member with id {id}."});
                    }
                    else
                    {
                        return Ok(result);
                    }                    
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

        private bool MemberExists(string id)
        {
            return unitOfWork.Members.Find(x => x.IdNumber == id).FirstOrDefault() != null ? true : false;
        }
    }
}