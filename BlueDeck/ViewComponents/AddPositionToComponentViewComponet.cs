using Microsoft.AspNetCore.Mvc;
using OrgChartDemo.Models.ViewModels;

namespace OrgChartDemo.ViewComponents
{
    public class AddPositionToComponentViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(AddPositionToComponentViewComponentViewModel vm)
        {            
            return View(vm);
        }
    }
}
