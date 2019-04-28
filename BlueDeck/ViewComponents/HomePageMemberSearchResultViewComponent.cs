using BlueDeck.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BlueDeck.ViewComponents
{
    public class HomePageMemberSearchResultViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(HomePageMemberSearchResultViewComponentViewModel vm)
        {            
            return View(vm);
        }
    }
}
