using Microsoft.AspNetCore.Mvc;
using System.Linq;
using OrgChartDemo.Models;
using OrgChartDemo.Models.ViewModels;
namespace OrgChartDemo.Components
{
    public class OrgChartOptionsMenuViewComponent : ViewComponent {
        private IComponentRepository repository;
        public OrgChartOptionsMenuViewComponent(IComponentRepository repo) {
            repository = repo;
        }

        public IViewComponentResult Invoke()
        {
            return View(new OrgChartOptionsViewModel { Components = repository.Components.ToList(), Members = repository.Members.ToList() });
        }
    }

    
}
