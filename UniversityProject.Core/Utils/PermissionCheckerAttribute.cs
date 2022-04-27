using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UniversityProject.Data.Consts;

namespace UniversityProject.Core.Utils
{
    public class PermissionCheckerAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public PermissionCheckerAttribute()
        {

        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var roleId = context.HttpContext.User.GetRoleId();
                if (roleId != Const.AdminRoleId)
                {
                    context.Result = new RedirectResult("/Login" + context.HttpContext.Request.Path);
                }
            }
            else
            {
                context.Result = new RedirectResult("/Login");
            }
        }
    }
}
