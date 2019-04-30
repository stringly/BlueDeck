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
    
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class MembersApiController : ControllerBase
    {
        private IUnitOfWork unitOfWork;

        public MembersApiController(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }

        // GET: api/MembersApi/GetAll
        [HttpGet("GetAll")]
        [AllowAnonymous]
        public IEnumerable<MemberSelectListItem> GetMembers()
        {
            
            return unitOfWork.Members.GetAllMemberSelectListItems();
        }

        // GET: api/MembersApi/SearchMembers/searchString
        [HttpGet("SearchMembers/{searchString}" )]
        public async Task<IActionResult> SearchMembers([FromRoute] string searchString)
        {
            IEnumerable<MemberSelectListItem> result = new List<MemberSelectListItem>();
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
                result = members.ToList().ConvertAll(x => new MemberSelectListItem(x));
            }
            else
            {
                return NotFound(new { status = "Not Found", message = "Search string parameter is required." });
            }
            if (result.Count() > 0)
            {
                return Ok(new { status = "Success", members = result });
            }
            else
            {
                return NotFound(new { status = "Not Found", message = $"No members match {searchString}" });
            }
        }

        // GET: api/MembersApi/5
        [HttpGet("GetMember/{id}")]
        public async Task<IActionResult> GetMember([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            

            if (!MemberExists(id))
            {
                return NotFound(new { status = "Not Found", message = $"No Member with id={id} exists." });
            }
            else
            {
                MemberApiResult result = await unitOfWork.Members.GetApiMember(id);            
                return Ok(new { status = "Success", member = result });
            }            
        }
             

        private bool MemberExists(int id)
        {
            return unitOfWork.Members.Get(id) != null ? true : false;
        }
    }
}