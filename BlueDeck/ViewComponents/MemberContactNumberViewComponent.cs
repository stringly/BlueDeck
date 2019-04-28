using Microsoft.AspNetCore.Mvc;
using BlueDeck.Models.ViewModels;

namespace BlueDeck.ViewComponents
{
    public class MemberContactNumberViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(MemberContactNumberViewComponentViewModel vm)
        {
            return View(vm);
        }
    }
}
