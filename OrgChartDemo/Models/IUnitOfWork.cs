using System;
using OrgChartDemo.Models.Repositories;

namespace OrgChartDemo.Models
{
    public interface IUnitOfWork : IDisposable
    {
        IPositionRepository Positions { get; }
        IComponentRepository Components { get; }
        IMemberRepository Members { get; }
        int Complete();
    }
}
