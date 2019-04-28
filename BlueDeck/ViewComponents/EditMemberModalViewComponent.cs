using Microsoft.AspNetCore.Mvc;
using OrgChartDemo.Models.ViewModels;

namespace OrgChartDemo.ViewComponents
{
    public class EditMemberModalViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(MemberAddEditViewModel vm)
        {
            return View(vm);
        }        
    }
}
