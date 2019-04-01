﻿using Microsoft.AspNetCore.Mvc;
using System.Linq;
using OrgChartDemo.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using OrgChartDemo.Models.ViewModels;

namespace OrgChartDemo.ViewComponents
{
    /// <summary>
    /// A ViewComponent that renders the Navigation Sidebar
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Mvc.ViewComponent" />
    public class NavigationMenuViewComponent : ViewComponent
    {
        /// <summary>
        /// Invokes the default <see cref="T:OrgChartDemo.ViewComponents.NavigationMenuViewComponent" />
        /// </summary>
        /// <returns></returns>              
        public IViewComponentResult Invoke()
        {  
            MainNavMenuViewModel vm = new MainNavMenuViewModel();
            vm.NavLinks = new List<string>()
            {
                "OrgChart", 
                "Positions", 
                "Members", 
                "Components",
            };
            if (User.IsInRole("ComponentAdmin"))
            {
                vm.NavLinks.Add("Roster");
            }
            if (User.IsInRole("GlobalAdmin"))
            {
                vm.NavLinks.Add("Admin");
            }
            
            return View(vm);
        }
    }


}
