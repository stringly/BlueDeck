using BlueDeck.Models;
using BlueDeck.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlueDeck.Models.Enums;

namespace BlueDeck.Persistence.Repositories
{
    public class MemberRoleTypeRepository : Repository<RoleType>, IMemberRoleTypeRepository
    {
        public MemberRoleTypeRepository(ApplicationDbContext context) : base(context)
        {
        }
        public ApplicationDbContext ApplicationDbContext {
            get { return Context as ApplicationDbContext; }
        }
    }
}
