using Microsoft.AspNetCore.Mvc;
using OrgChartDemo.Models.ViewModels;

namespace OrgChartDemo.ViewComponents
{
    public class ComponentLineupViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ComponentLineupViewComponentViewModel vm)
        {
            return View(vm);
        }
    }
}