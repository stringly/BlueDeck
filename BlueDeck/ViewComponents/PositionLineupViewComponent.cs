using Microsoft.AspNetCore.Mvc;
using BlueDeck.Models.ViewModels;

namespace BlueDeck.ViewComponents
{
    public class PositionLineupViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(PositionLineupViewComponentViewModel vm)
        {
            return View(vm);
        }
    }
}
