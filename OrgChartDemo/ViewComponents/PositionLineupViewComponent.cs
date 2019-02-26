using Microsoft.AspNetCore.Mvc;
using OrgChartDemo.Models.ViewModels;

namespace OrgChartDemo.ViewComponents
{
    public class PositionLineupViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(PositionLineupViewComponentViewModel vm)
        {
            return View(vm);
        }
    }
}
