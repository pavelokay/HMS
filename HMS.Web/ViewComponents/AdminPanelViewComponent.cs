using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace HMS.Web.ViewComponents
{
    public class AdminPanelViewComponent : ViewComponent
    {
        [Authorize]
        public IViewComponentResult Invoke()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("admin"))
                {
                    return View("~/Views/AdminPanelPartial/PartialAdminPanel.cshtml");
                }
                else if (User.IsInRole("accountant"))
                {
                    return View("~/Views/AdminPanelPartial/PartialAccountantPanel.cshtml");
                }
                else if (User.IsInRole("accommodationOfficer"))
                {
                    return View("~/Views/AdminPanelPartial/PartialAccommodationOfficerPanel.cshtml");
                }
            }
            return View();
        }

    }
}
