using Microsoft.AspNetCore.Mvc;
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
        private readonly string UserName;

        public NavigationMenuViewComponent(IUnitOfWork unitOfWork)
        {
            UserName = unitOfWork.CurrentUser();
        }

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
                "Roster",
            };
            vm.UserName = UserName;
            return View(vm);
        }
    }


}
