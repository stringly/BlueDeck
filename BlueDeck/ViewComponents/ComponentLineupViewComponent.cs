using Microsoft.AspNetCore.Mvc;
using BlueDeck.Models.ViewModels;

namespace BlueDeck.ViewComponents
{
    public class ComponentLineupViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ComponentLineupViewComponentViewModel vm)
        {
            return View(vm);
        }
    }
}