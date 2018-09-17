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
        public IEnumerable<ChartableComponent> ChartableComponents => GetOrgChartComponentsWithoutMembers();                     
        public IEnumerable<Component> Components => context.Components;
        public IEnumerable<Position> Positions => context.Positions;
        public IEnumerable<Member> Members => context.Members;

        // not implemented
        //public IEnumerable<ChartableComponentWithManager> GetChartableComponentsWithManagers() { }


        public IEnumerable<ChartableComponent> GetOrgChartComponentsWithoutMembers()
        {
            List<Component> components = context.Components.ToList();
            List<ChartableComponent> results = new List<ChartableComponent>();
            foreach (Component c in components)
            {
                ChartableComponent n = new ChartableComponent
                {
                    id = c.ComponentId,
                    parentid = c?.ParentComponent?.ComponentId,
                    componentName = c.Name
                };
                results.Add(n);
            }
            return results;
        }


    }
}
