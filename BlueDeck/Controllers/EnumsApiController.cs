using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlueDeck.Models.APIModels;
using BlueDeck.Models;
using Microsoft.AspNetCore.Authorization;

namespace BlueDeck.Controllers
{
    /// <summary>
    /// API Controller that retrieves BlueDeck Enumerables
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class EnumsApiController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumsApiController"/> class.
        /// </summary>
        /// <param name="unit">The unit.</param>
        public EnumsApiController(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }

        /// <summary>
        /// Gets a List of all BlueDeck enumerable types
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /EnumsApi/GetAll
        ///     {
        ///         "genders": [
        ///             {
        ///                 "genderId": 1,
        ///                 "genderFullName": "Unknown",
        ///                 "abbreviation": "U",
        ///                 "members": null
        ///             },
        ///             {
        ///                 "genderId": 2,
        ///                 "genderFullName": "Male",
        ///                 "abbreviation": "M",
        ///                 "members": null
        ///             }
        ///         ],
        ///         "ranks": [
        ///             {
        ///                 "rankId": 1,
        ///                 "rankFullName": "Police Officer",
        ///                 "rankShort": "P/O",
        ///                 "payGrade": "L01",
        ///                 "isSworn": true,
        ///                 "members": null
        ///             },
        ///             {
        ///                 "rankId": 2,
        ///                 "rankFullName": "Police Officer First Class",
        ///                 "rankShort": "POFC",
        ///                 "payGrade": "L02",
        ///                 "isSworn": true,
        ///                 "members": null
        ///             }
        ///         ],
        ///         ...              
        ///     }
        ///         
        /// Use this method to retrieve a collection of all BlueDeck Enumerable types
        /// </remarks>
        /// <returns>A collection of all BlueDeck Enumerable types</returns>
        /// <response code="200">Returns a JSON object containing all BlueDeck Enumerable types</response>
        [HttpGet("GetAll")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                EnumApiCollectionResult response = new EnumApiCollectionResult();
                response.Genders = unitOfWork.MemberGenders.GetAll().ToList().ConvertAll(x => new GenderApiResult(x));
                response.Ranks = unitOfWork.MemberRanks.GetAll().ToList().ConvertAll(x => new RankApiResult(x));
                response.Races = unitOfWork.MemberRaces.GetAll().ToList().ConvertAll(x => new RaceApiResult(x));;
                response.DutyStatuses = unitOfWork.MemberDutyStatus.GetAll().ToList().ConvertAll(x => new DutyStatusApiResult(x));
                response.PhoneTypes = unitOfWork.PhoneNumberTypes.GetAll().ToList().ConvertAll(x => new PhoneNumberTypeApiResult(x));
                response.RoleTypes = unitOfWork.Members.GetMemberRoles().ToList().ConvertAll(x => new RoleTypeApiResult(x));;
                response.AppStatuses = unitOfWork.AppStatuses.GetAll().ToList().ConvertAll(x => new AppStatusApiResult(x));;
                return Ok(response);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "The service encountered an error." });
            }
        }

        /// <summary>
        /// Gets a list of BlueDeck Gender Types.
        /// </summary>
        /// <remarks>
        /// Use this method to create Select Lists that allow a user to select one of the available BlueDeck genders.
        /// Sample request:
        /// 
        ///     GET: EnumsApi/GetGenders
        ///     [
        ///         {
        ///             "genderId": 1,
        ///             "genderName": "Unknown",
        ///             "abbreviation": "U"
        ///         },
        ///         {
        ///             "genderId": 2,
        ///             "genderName": "Male",
        ///             "abbreviation": "M"
        ///         },
        ///         {
        ///             "genderId": 3,
        ///             "genderName": "Female",
        ///             "abbreviation": "F"
        ///         }
        ///     ]
        /// </remarks>
        /// <returns>A list of <see cref="GenderApiResult"/> of all current gender types in the BlueDeck Database</returns>
        /// <response code="200">Returns a JSON Object containing all current BlueDeck gender enumerables.</response>
        [HttpGet("GetGenders")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetGenders()
        {
            try
            {
                return Ok(unitOfWork.MemberGenders.GetAll().ToList().ConvertAll(x => new GenderApiResult(x)));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "The service encountered an error." });
            }
        }

        /// <summary>
        /// Gets a list of BlueDeck Rank Types.
        /// </summary>
        /// <remarks>
        /// Use this method to create Select Lists that allow a user to select one of the available BlueDeck ranks.
        /// Sample request:
        /// 
        ///     GET: EnumsApi/GetRanks
        ///     [
        ///         {
        ///             "rankId": 1,
        ///             "rankName": "Police Officer ",
        ///             "rankShort": "P/O",
        ///             "payGrade": "L01",
        ///             "isSworn": true
        ///         },
        ///         {
        ///             "rankId": 2,
        ///             "rankName": "Police Officer First Class",
        ///             "rankShort": "POFC",
        ///             "payGrade": "L02",
        ///             "isSworn": true
        ///         }
        ///     ]
        /// </remarks>
        /// <returns>A list of <see cref="RankApiResult"/> of all current rank types in the BlueDeck Database</returns>
        /// <response code="200">Returns a JSON Object containing all current BlueDeck rank enumerables.</response>
        [HttpGet("GetRanks")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetRanks()
        {
            try
            {
                return Ok(unitOfWork.MemberRanks.GetAll().ToList().ConvertAll(x => new RankApiResult(x)));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "The service encountered an error." });
            }
        }

        /// <summary>
        /// Gets a list of BlueDeck Race Types.
        /// </summary>
        /// <remarks>
        /// Use this method to create Select Lists that allow a user to select one of the available BlueDeck races.
        /// Sample request:
        /// 
        ///     GET: EnumsApi/GetRaces
        ///     [
        ///         {
        ///             "raceId": 1,
        ///             "raceName": "Black",
        ///             "abbreviation": "B"
        ///         },
        ///         {
        ///             "raceId": 2,
        ///             "raceName": "White",
        ///             "abbreviation": "W"
        ///         },
        ///         {
        ///             "raceId": 3,
        ///             "raceName": "Asian",
        ///             "abbreviation": "A"
        ///         }
        ///     ]
        /// </remarks>
        /// <returns>A list of <see cref="RaceApiResult"/> of all current race types in the BlueDeck Database</returns>
        /// <response code="200">Returns a JSON Object containing all current BlueDeck race enumerables.</response>
        [HttpGet("GetRaces")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetRaces()
        {
            try
            {
                return Ok(unitOfWork.MemberRaces.GetAll().ToList().ConvertAll(x => new RaceApiResult(x)));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "The service encountered an error." });
            }
        }

        /// <summary>
        /// Gets a list of BlueDeck Duty Status Types.
        /// </summary>
        /// <remarks>
        /// Use this method to create Select Lists that allow a user to select one of the available BlueDeck Duty Statuses.
        /// Sample request:
        /// 
        ///     GET: EnumsApi/GetDutyStatuses
        ///     [
        ///          {
        ///            "name": "Full Duty",
        ///            "abbreviation": "F",
        ///            "hasPolicePower": true
        ///          },
        ///          {
        ///            "name": "Light Duty",
        ///            "abbreviation": "L",
        ///            "hasPolicePower": true
        ///          },
        ///          {
        ///            "name": "No Duty",
        ///            "abbreviation": "N",
        ///            "hasPolicePower": true
        ///          }
        ///     ]
        /// </remarks>
        /// <returns>A list of <see cref="DutyStatusApiResult"/> of all current duty status types in the BlueDeck Database</returns>
        /// <response code="200">Returns a JSON Object containing all current BlueDeck duty status enumerables.</response>
        [HttpGet("GetDutyStatuses")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetDutyStatuses()
        {
            try
            {
                return Ok(unitOfWork.MemberDutyStatus.GetAll().ToList().ConvertAll(x => new DutyStatusApiResult(x)));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "The service encountered an error." });
            }
        }

        /// <summary>
        /// Gets a list of BlueDeck Phone Number Types.
        /// </summary>
        /// <remarks>
        /// Use this method to create Select Lists that allow a user to select one of the available BlueDeck Phone Number Types.
        /// Sample request:
        /// 
        ///     GET: EnumsApi/GetPhoneNumberTypes
        ///     [
        ///         {
        ///             "phoneNumberTypeId": 1,
        ///             "phoneNumberTypeName": "Cell (Personal)"
        ///         },
        ///         {
        ///             "phoneNumberTypeId": 2,
        ///             "phoneNumberTypeName": "Cell (Work)"
        ///         },
        ///         {
        ///             "phoneNumberTypeId": 3,
        ///             "phoneNumberTypeName": "Office"
        ///         }
        ///     ]
        /// </remarks>
        /// <returns>A list of <see cref="PhoneNumberTypeApiResult"/> of all current phone number types in the BlueDeck Database</returns>
        /// <response code="200">Returns a JSON Object containing all current BlueDeck phone number enumerables.</response>
        [HttpGet("GetPhoneNumberTypes")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetPhoneNumberTypes()
        {
            try
            {
                return Ok(unitOfWork.PhoneNumberTypes.GetAll().ToList().ConvertAll(x => new PhoneNumberTypeApiResult(x)));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "The service encountered an error." });
            }
        }
        
        /// <summary>
        /// Gets a list of BlueDeck Member Role Types.
        /// </summary>
        /// <remarks>
        /// Use this method to create Select Lists that allow a user to select one of the available BlueDeck Member Role Types.
        /// Sample request:
        /// 
        ///     GET: EnumsApi/GetRoleTypes
        ///     [
        ///         {
        ///             "roleTypeId": 1,
        ///             "roleTypeName": "GlobalAdmin"
        ///         },
        ///         {
        ///             "roleTypeId": 2,
        ///             "roleTypeName": "ComponentAdmin"
        ///         },
        ///         {
        ///             "roleTypeId": 3,
        ///             "roleTypeName": "User"
        ///         }
        ///     ]
        /// </remarks>
        /// <returns>A list of <see cref="RoleTypeApiResult"/> of all current member role types types in the BlueDeck Database</returns>
        /// <response code="200">Returns a JSON Object containing all current BlueDeck member role type enumerables.</response>
        [HttpGet("GetRoleTypes")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetRoleTypes()
        {
            try
            {
                return Ok(unitOfWork.RoleTypes.GetAll().ToList().ConvertAll(x => new RoleTypeApiResult(x)));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "The service encountered an error." });
            }
        }

        /// <summary>
        /// Gets a list of BlueDeck Application Status Types.
        /// </summary>
        /// <remarks>
        /// Use this method to create Select Lists that allow a user to select one of the available BlueDeck Member Account application status Types.
        /// Sample request:
        /// 
        ///     GET: EnumsApi/GetAppStatuses
        ///     [
        ///         {
        ///             "appStatusId": 1,
        ///             "statusName": "New"
        ///         },
        ///         {
        ///             "appStatusId": 2,
        ///             "statusName": "Pending"
        ///         },
        ///         {
        ///             "appStatusId": 3,
        ///             "statusName": "Active"
        ///         }
        ///     ]
        /// </remarks>
        /// <returns>A list of <see cref="AppStatusApiResult"/> of all current member account application status types in the BlueDeck Database</returns>
        /// <response code="200">Returns a JSON Object containing all current BlueDeck member account application status type enumerables.</response>
        [HttpGet("GetAppStatuses")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetAppStatuses()
        {
            try
            {
                return Ok(unitOfWork.AppStatuses.GetAll().ToList().ConvertAll(x => new AppStatusApiResult(x)));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "The service encountered an error." });
            }
        }
    }
}