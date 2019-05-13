using Microsoft.AspNetCore.Mvc;
using BlueDeck.Models.ViewModels;
using BlueDeck.Models;
namespace BlueDeck.ViewComponents
{
    public class ComponentDemographicTableViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ComponentDemographicTableViewComponentViewModel vm)
        {            
            return View(vm);
        }
    }
}
