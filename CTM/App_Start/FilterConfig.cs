using System.Web;
using System.Web.Mvc;
using CTM;
using CTM.Codes.Attributes;


namespace CTM
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthorizeAttribute());
            filters.Add(new SystemErrorHandleAttribute());
        }

    }

}
