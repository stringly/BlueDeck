using OrgChartDemo.Models;
using OrgChartDemo.Models.Repositories;
using OrgChartDemo.Persistence.Repositories;

namespace OrgChartDemo.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Positions = new PositionRepository(_context);
            Components = new ComponentRepository(_context);
            Members = new MemberRepository(_context);
        }

        public IPositionRepository Positions { get; private set; }
        public IComponentRepository Components { get; private set; }
        public IMemberRepository Members { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
