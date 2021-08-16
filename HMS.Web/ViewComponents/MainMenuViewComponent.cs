using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Web.ViewComponents
{
    public class MainMenuViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("admin") || User.IsInRole("accommodationOfficer") || User.IsInRole("accountant"))
                {
                    return View("~/Views/MainMenuPartial/PartialAdminMenu.cshtml");
                }
                else
                {
                    return View("~/Views/MainMenuPartial/PartialUserMenu.cshtml");
                }
            }
            return View("~/Views/MainMenuPartial/PartialUnonimousMenu.cshtml");
        }
    }
}
