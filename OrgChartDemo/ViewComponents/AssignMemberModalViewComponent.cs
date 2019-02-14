using Microsoft.AspNetCore.Mvc;
using OrgChartDemo.Models.ViewModels;

namespace OrgChartDemo.ViewComponents
{
    public class AssignMemberModalViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(AssignMemberModalViewComponentViewModel vm)
        {
            return View(vm);
        }
    }
}
