using Microsoft.AspNetCore.Mvc;
using OrgChartDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.ViewComponents
{
    public class RosterManagerViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(List<Component> componentList)
        {           
            return View(componentList);
        }
    }
}
