using Microsoft.AspNetCore.Mvc;
using OrgChartDemo.Models.ViewModels;

namespace OrgChartDemo.ViewComponents
{
    public class PositionMemberLineupViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(MemberLineupViewComponentViewModel vm)
        {
            return View(vm);
        }
    }
}
