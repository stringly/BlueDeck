using Microsoft.AspNetCore.Mvc;
using BlueDeck.Models.ViewModels;


namespace BlueDeck.ViewComponents
{
    public class ComponentAddEditModalViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ComponentWithComponentListViewModel vm)
        {
            return View(vm);
        }
    }
}
