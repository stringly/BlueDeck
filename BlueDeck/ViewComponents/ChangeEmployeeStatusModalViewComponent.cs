using Microsoft.AspNetCore.Mvc;
using OrgChartDemo.Models.ViewModels;

namespace OrgChartDemo.ViewComponents
{
    public class ChangeEmployeeStatusModalViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ChangeEmployeeStatusModalViewComponentViewModel vm)
        {
            return View(vm);
        }
    }
}
