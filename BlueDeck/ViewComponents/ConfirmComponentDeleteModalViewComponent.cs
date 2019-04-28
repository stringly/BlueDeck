using Microsoft.AspNetCore.Mvc;
using BlueDeck.Models;


namespace BlueDeck.ViewComponents
{
    public class ConfirmComponentDeleteModalViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(Component c)
        {
            return View(c);
        }
    }
}
