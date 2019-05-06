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
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class EnumsApiController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public EnumsApiController(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }

        /// <summary>
        /// Gets a List of all BlueDeck enumberable types
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /EnumsApi/GetAll
        ///     {
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
                response.Genders = unitOfWork.MemberGenders.GetAll();
                response.Ranks = unitOfWork.MemberRanks.GetAll();
                response.Races = unitOfWork.MemberRaces.GetAll();
                response.DutyStatuses = unitOfWork.MemberDutyStatus.GetAll();
                response.RoleTypes = unitOfWork.Members.GetMemberRoles();
                response.AppStatuses = unitOfWork.AppStatuses.GetAll();
                return Ok(response);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "The service encountered an error." });
            }
        }
    }
}