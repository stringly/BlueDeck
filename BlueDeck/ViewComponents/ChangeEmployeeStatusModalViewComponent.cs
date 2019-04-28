using Microsoft.AspNetCore.Mvc;
using BlueDeck.Models.ViewModels;

namespace BlueDeck.ViewComponents
{
    public class ChangeEmployeeStatusModalViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ChangeEmployeeStatusModalViewComponentViewModel vm)
        {
            return View(vm);
        }
    }
}
