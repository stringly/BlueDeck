using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrgChartDemo.Models.ViewModels;

namespace OrgChartDemo.ViewComponents
{
    public class PositionLineupViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(PositionLineupViewComponentViewModel vm)
        {
            return View(vm);
        }
    }
}
