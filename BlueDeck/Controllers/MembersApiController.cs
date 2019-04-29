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

namespace BlueDeck.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class MembersApiController : ControllerBase
    {
        private IUnitOfWork unitOfWork;

        public MembersApiController(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }

        // GET: api/MembersApi
        [HttpGet]
        public IEnumerable<Member> GetMembers()
        {
            return unitOfWork.Members.GetAll();
        }

        // GET: api/MembersApi/5
        [HttpGet("{id}")]
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

        //// PUT: api/MembersApi/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutMember([FromRoute] int id, [FromBody] Member member)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != member.MemberId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(member).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!MemberExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/MembersApi
        //[HttpPost]
        //public async Task<IActionResult> PostMember([FromBody] Member member)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    _context.Members.Add(member);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetMember", new { id = member.MemberId }, member);
        //}

        //// DELETE: api/MembersApi/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteMember([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var member = await _context.Members.FindAsync(id);
        //    if (member == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Members.Remove(member);
        //    await _context.SaveChangesAsync();

        //    return Ok(member);
        //}

        private bool MemberExists(int id)
        {
            return unitOfWork.Members.Get(id) != null ? true : false;
        }
    }
}