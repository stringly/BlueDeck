using Microsoft.AspNetCore.Mvc;
using OrgChartDemo.Models;
using OrgChartDemo.Models.ViewModels;
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
            RosterManagerViewComponentViewModel vm = new RosterManagerViewComponentViewModel(componentList);
            return View(vm);
        }
    }
}
