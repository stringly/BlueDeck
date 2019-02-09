using Microsoft.AspNetCore.Mvc;
using OrgChartDemo.Models.ViewModels;

namespace OrgChartDemo.ViewComponents
{
    public class EditMemberModalViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(EditMemberModalViewComponentViewModel vm)
        {
            return View(vm);
        }        
    }
}
