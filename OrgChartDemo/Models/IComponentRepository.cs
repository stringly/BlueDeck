using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models {
    public interface IComponentRepository {
        IEnumerable<ChartableComponent> ChartableComponents { get; }
        IEnumerable<Component> Components { get; }
        IEnumerable<Position> Positions { get; }
        IEnumerable<Member> Members { get; }
}
}
