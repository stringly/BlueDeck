using Microsoft.AspNetCore.Mvc;
using OrgChartDemo.Models;


namespace OrgChartDemo.ViewComponents
{
    public class ConfirmComponentDeleteModalViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(Component c)
        {
            return View(c);
        }
    }
}
