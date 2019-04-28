using Microsoft.AspNetCore.Mvc;
using OrgChartDemo.Models.ViewModels;


namespace OrgChartDemo.ViewComponents
{
    public class ComponentAddEditModalViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ComponentWithComponentListViewModel vm)
        {
            return View(vm);
        }
    }
}
