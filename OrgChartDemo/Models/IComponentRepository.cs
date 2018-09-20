using OrgChartDemo.Models.ViewModels;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models {
    /// <summary>
    /// Interface for OrgChartDemo Repository to facilitate dependency injection
    /// </summary>
    public interface IComponentRepository {        
        List<Position> Positions { get; }
        List<Component> Components { get; }
        List<Member> Members { get; }
        IEnumerable<ChartableComponent> GetOrgChartComponentsWithoutMembers();
        IEnumerable<ChartableComponentWithMember> GetOrgChartComponentsWithMembers();
        IEnumerable<PositionWithMemberCountItem> GetPositionListWithMemberCount();
        void AddPosition(Position p);
        void RemovePosition(int PostionIdToRemove);
        void EditPosition(Position p);

    }
}
