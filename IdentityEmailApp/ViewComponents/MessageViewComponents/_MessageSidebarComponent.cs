using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.ViewComponents.MessageViewComponents
{
    public class _MessageSidebarComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
