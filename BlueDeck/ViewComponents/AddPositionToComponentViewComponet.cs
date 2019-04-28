using Microsoft.AspNetCore.Mvc;
using BlueDeck.Models.ViewModels;

namespace BlueDeck.ViewComponents
{
    public class AddPositionToComponentViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(AddPositionToComponentViewComponentViewModel vm)
        {            
            return View(vm);
        }
    }
}
