using Microsoft.AspNetCore.Mvc;
using System.Linq;
using OrgChartDemo.Models;
using System.Collections.Generic;

namespace OrgChartDemo.Components
{
    /// <summary>
    /// A ViewComponent that renders the Navigation Sidebar
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Mvc.ViewComponent" />
    public class NavigationMenuViewComponent : ViewComponent
    {
        /// <summary>
        /// Invokes the default <see cref="T:OrgChartDemo.Components.NavigationMenuViewComponent" />
        /// </summary>
        /// <returns></returns>
        public IViewComponentResult Invoke()
        {
            return View(new List<string> { "OrgChart", "Positions", "Members", "Components" });
        }
    }
}
