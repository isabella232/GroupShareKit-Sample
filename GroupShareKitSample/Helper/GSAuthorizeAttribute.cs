using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GroupShareKitSample.Helper
{
    public class GSAuthorizeAttribute: AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var result =  base.AuthorizeCore(httpContext);
            var gsClient = HelperMethods.GetCurrentGsClient(httpContext.User);

            if (gsClient == null) return false;

            return result;
        }
    }
}