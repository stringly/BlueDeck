using Microsoft.AspNetCore.Mvc;
using System.Linq;
using OrgChartDemo.Models;
using System.Collections.Generic;

namespace OrgChartDemo.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        
        public IViewComponentResult Invoke()
        {
            return View(new List<string> { "OrgChart", "Positions", "Members", "Components" });
        }
    }
}
