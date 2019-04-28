using Microsoft.AspNetCore.Mvc;
using BlueDeck.Models.ViewModels;

namespace BlueDeck.ViewComponents
{
    public class EditMemberModalViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(MemberAddEditViewModel vm)
        {
            return View(vm);
        }        
    }
}
