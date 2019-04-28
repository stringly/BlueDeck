using Microsoft.AspNetCore.Mvc;
using BlueDeck.Models.ViewModels;

namespace BlueDeck.ViewComponents
{
    public class AssignMemberModalViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(AssignMemberModalViewComponentViewModel vm)
        {
            return View(vm);
        }
    }
}
