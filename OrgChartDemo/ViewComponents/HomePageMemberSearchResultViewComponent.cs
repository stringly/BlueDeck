using OrgChartDemo.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace OrgChartDemo.ViewComponents
{
    public class HomePageMemberSearchResultViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(HomePageMemberSearchResultViewComponentViewModel vm)
        {            
            return View(vm);
        }
    }
}
