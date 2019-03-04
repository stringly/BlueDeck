using Microsoft.AspNetCore.Mvc;
using OrgChartDemo.Models.ViewModels;

namespace OrgChartDemo.ViewComponents
{
    public class MemberContactNumberViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(MemberContactNumberViewComponentViewModel vm)
        {
            return View(vm);
        }
    }
}
