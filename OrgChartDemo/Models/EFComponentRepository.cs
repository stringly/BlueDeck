using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models {
    public class EFComponentRepository : IComponentRepository {
        private ApplicationDbContext context;

        public EFComponentRepository(ApplicationDbContext ctx) {
            context = ctx;
        }
        public IEnumerable<OrgChartComponentWithMember> OrgChartComponents { get; }

        // Re-wire OrgComponents to map to entities

        //public IEnumerable<OrgChartComponentWithMember> OrgChartComponents => new List<OrgChartComponentWithMember> {
        //    new OrgChartComponentWithMember {
        //        id = 1,
        //        parentid = null,
        //        componentName = "District I Commander's Office",
        //        memberId = 1,
        //        memberName = "Maj. Rosa Guixens #2013",
        //        contactNumber = "(301) 699-2630",
        //        email = "rmguixens@co.pg.md.us"
        //    },


        public IEnumerable<Component> Components => context.Components;
        public IEnumerable<Position> Positions => context.Positions;
        public IEnumerable<Member> Members => context.Members;
    }
}
