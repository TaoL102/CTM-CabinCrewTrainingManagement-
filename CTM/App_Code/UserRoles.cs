using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using CTMLocalizationLib.Resources;

namespace CTM
{
    [Flags]
    public enum UserRole
    {

        [Display(Description = "RefresherTrainingAdmin", ResourceType = typeof(ConstModels))]
        RefresherTrainingAdmin = 1 << 1,
        [Display(Description = "EnglishTestAdmin", ResourceType = typeof(ConstModels))]
        EnglishTestAdmin = 1 << 2,
        [Display(Description = "DangerousGoodsTrainingAdmin", ResourceType = typeof(ConstModels))]
        DangerousGoodsTrainingAdmin = 1 << 3,
        [Display(Description = "PromotionAdmin", ResourceType = typeof(ConstModels))]
        PromotionAdmin = 1 << 4,
        [Display(Description = "SuperAdmin", ResourceType = typeof(ConstModels))]
        SuperAdmin = (
            EnglishTestAdmin |
            RefresherTrainingAdmin |
            DangerousGoodsTrainingAdmin |
            PromotionAdmin
            )
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {

        public UserRole UserRole { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (UserRole != 0)
                Roles = UserRole.ToString();

            base.OnAuthorization(filterContext);
        }

    }

    /// <summary>
    /// Custom Principal , 
    /// Reference: http://stackoverflow.com/questions/2828444/non-string-role-names-in-asp-net-mvc
    /// </summary>
    public static class PrincipalExtensions
    {

        public static bool IsInRole(this IPrincipal user, UserRole userRole)
        {

            var roles = userRole.ToString().Split(',').Select(x => x.Trim());
            foreach (var role in roles)
            {
                if (user.IsInRole(role))
                    return true;
            }

            return false;
        }
    }
}