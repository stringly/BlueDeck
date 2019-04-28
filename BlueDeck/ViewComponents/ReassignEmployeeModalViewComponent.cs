using Microsoft.AspNetCore.Mvc;
using BlueDeck.Models;
using BlueDeck.Models.Types;
using BlueDeck.Models.ViewModels;
using System;
using System.Collections.Generic;

namespace BlueDeck.ViewComponents
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
