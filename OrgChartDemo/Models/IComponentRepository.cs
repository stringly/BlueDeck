using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models {
    public interface IComponentRepository {
        IEnumerable<OrgChartComponentWithMember> OrgChartComponents { get; }
    }
}
