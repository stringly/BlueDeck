using Microsoft.AspNetCore.Mvc;
using BlueDeck.Models;
using BlueDeck.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.ViewComponents
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
