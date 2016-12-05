using BBBZ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BBBZ.Helper
{
    public class HasPermissionAttribute: ActionFilterAttribute
    {
        private string _permission;

        public HasPermissionAttribute(string permission)
        {
            this._permission = permission;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!CHECK_IF_USER_OR_ROLE_HAS_PERMISSION(_permission))
            {
                // If this user does not have the required permission then redirect to login page
                var url = new UrlHelper(filterContext.RequestContext);
                var loginUrl = url.Content("/Account/Login");
                filterContext.HttpContext.Response.Redirect(loginUrl, true);
            }
        }

        protected ApplicationDbContext db;
        public bool CHECK_IF_USER_OR_ROLE_HAS_PERMISSION(string p)
        {
            return true;
        }
    }
}