using Microsoft.AspNetCore.Mvc;
using OrgChartDemo.Models;
using OrgChartDemo.Models.Types;
using OrgChartDemo.Models.ViewModels;
using System;
using System.Collections.Generic;

namespace OrgChartDemo.ViewComponents
{
    public class ReassignEmployeeModalViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(Member m, List<PositionSelectListItem> p, int s)
        {
            ReassignEmployeeModalViewComponentViewModel vm = new ReassignEmployeeModalViewComponentViewModel(m, p, s);
            return View(vm);
        }
    }
}
