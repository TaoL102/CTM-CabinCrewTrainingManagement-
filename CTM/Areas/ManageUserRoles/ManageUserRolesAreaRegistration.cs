using System.Web.Mvc;

namespace CTM.Areas.ManageUserRoles
{
    public class ManageUserRolesAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ManageUserRoles";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ManageUserRoles_default",
                "ManageUserRoles/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}