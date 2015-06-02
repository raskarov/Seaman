using System;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Seaman.Web.Code
{
    public static class ApiControllerExtensions
    {
        public static Int32 GetUserId(this ApiController controller)
        {
            return controller.User.Identity.GetUserId<Int32>();
        }

        public static IAuthenticationManager GetAuthentication(this ApiController controller)
        {
            return controller.Request.GetOwinContext().Authentication;
        }
    }

}