using Microsoft.EntityFrameworkCore;
using OrgChartDemo.Models;
using OrgChartDemo.Models.Repositories;
using OrgChartDemo.Models.Types;
using OrgChartDemo.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;


namespace OrgChartDemo.Persistence.Repositories
{
    /// <summary>
    /// A repository for the Member entity
    /// </summary>
    /// <seealso cref="T:OrgChartDemo.Persistence.Repositories.Repository{OrgChartDemo.Models.Member}" />
    /// <seealso cref="T:OrgChartDemo.Models.Repositories.IMemberRepository" />
    public class MemberRepository : Repository<Member>, IMemberRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:OrgChartDemo.Persistence.Repositories.MemberRepository"/> class.
        /// </summary>
        /// <param name="context">The <see cref="T:OrgChartDemo.Models.ApplicationDbContext"/>.</param>
        public MemberRepository(ApplicationDbContext context)
         : base(context)
        {
        }

        /// <summary>
        /// Gets the members with positions.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Member> GetMembersWithPositions()
        {
            return ApplicationDbContext.Members
                .Include(c => c.Rank)
                .Include(c => c.Position)
                .ThenInclude(c => c.ParentComponent)
                .ToList();
        }

        public Member GetMemberWithPosition(int memberId)
        {
            return ApplicationDbContext.Members
                .Where(x => x.MemberId == memberId)
                .Include(x => x.Position)
                    .ThenInclude(x => x.ParentComponent)
                .Include(x => x.Gender)
                .Include(x => x.Race)
                .Include(x => x.Rank)
                .Include(x => x.DutyStatus)                
                .FirstOrDefault();
        }

        public Member GetMemberWithDemographicsAndDutyStatus(int memberId)
        {
            return ApplicationDbContext.Members
                .Where(x => x.MemberId == memberId)
                .Include(x => x.Position)
                .Include(x => x.PhoneNumbers)
                    .ThenInclude(x => x.Type)                
                .Include(x => x.DutyStatus)                    
                .Include(x => x.Gender)                    
                .Include(x => x.Race)                    
                .Include(x => x.Rank)                    
                .FirstOrDefault();
        }

        public IEnumerable<MemberSelectListItem> GetAllMemberSelectListItems()
        {
            return ApplicationDbContext.Members
                .Include(x => x.Rank)
                .OrderByDescending(x => x.IdNumber)
                .ToList().ConvertAll(x => new MemberSelectListItem { MemberId = x.MemberId, MemberName = x.GetTitleName() });
        }

        public void UpdateMember(MemberAddEditViewModel form)
        {
            Member m;
            if (form.MemberId != null)
            {
                    m = ApplicationDbContext.Members
                .Include(x => x.PhoneNumbers)
                .FirstOrDefault(x => x.MemberId == form.MemberId);
            }
            else
            {
                m = new Member();
            }
            
            m.Position = ApplicationDbContext.Positions.FirstOrDefault(x => x.PositionId == form.PositionId);
            m.Rank = ApplicationDbContext.MemberRanks.SingleOrDefault(x => x.RankId == form.MemberRank);
            m.Gender = ApplicationDbContext.MemberGender.SingleOrDefault(x => x.GenderId == form.MemberGender);
            m.Race = ApplicationDbContext.MemberRace.SingleOrDefault(x => x.MemberRaceId == form.MemberRace);
            m.DutyStatus = ApplicationDbContext.DutyStatus.SingleOrDefault(x => x.DutyStatusId == form.DutyStatusId);
            m.Email = form.Email;
            m.FirstName = form.FirstName;
            m.IdNumber = form.IdNumber;
            m.MiddleName = form.MiddleName;
            m.LastName = form.LastName;
            foreach(MemberContactNumber n in form.ContactNumbers)
            {
                if (n.MemberContactNumberId != 0)
                {
                    if (n.ToDelete == true)
                    {
                        MemberContactNumber toRemove = ApplicationDbContext.ContactNumbers.Where(x => x.MemberContactNumberId == n.MemberContactNumberId).FirstOrDefault();

                        ApplicationDbContext.ContactNumbers.Remove(toRemove);
                    }
                    else
                    {
                        MemberContactNumber toUpdate = m.PhoneNumbers.FirstOrDefault(x => x.MemberContactNumberId == n.MemberContactNumberId);
                        toUpdate.PhoneNumber = n.PhoneNumber;
                        toUpdate.Type = ApplicationDbContext.PhoneNumberTypes.FirstOrDefault(x => x.PhoneNumberTypeId == n.Type.PhoneNumberTypeId);
                    }
                }
                else if (n.PhoneNumber != null)
                {
                    MemberContactNumber toAdd = new MemberContactNumber()
                    {
                        Member = m,
                        PhoneNumber = n.PhoneNumber,
                        Type = ApplicationDbContext.PhoneNumberTypes.FirstOrDefault(x => x.PhoneNumberTypeId == n.Type.PhoneNumberTypeId)
                    };
                    m.PhoneNumbers.Add(toAdd);
                }
            }
            if (form.MemberId == null)
            {
                ApplicationDbContext.Members.Add(m);
            }
        }

        public void Remove(int memberId)
        {
            Member m = ApplicationDbContext.Members
                .Include(x => x.PhoneNumbers)
                .FirstOrDefault(x => x.MemberId == memberId);
            ApplicationDbContext.ContactNumbers.RemoveRange(m.PhoneNumbers);
            ApplicationDbContext.Members.Remove(m);
        }
        /// <summary>
        /// Gets the application database context.
        /// </summary>
        /// <value>
        /// The application database context.
        /// </value>        
        public ApplicationDbContext ApplicationDbContext {
            get { return Context as ApplicationDbContext; }
        }
    }
}
