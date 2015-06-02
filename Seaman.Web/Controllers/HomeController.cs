using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.Owin.Security.Cookies;

namespace Seaman.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult Logout()
        {
            Request.GetOwinContext().Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Redirect("~/");
        }
    }
}
