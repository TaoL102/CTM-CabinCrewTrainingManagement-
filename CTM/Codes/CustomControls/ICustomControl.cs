using System.Web;

namespace CTM.Codes.CustomControls
{
    public interface ICustomControl : IHtmlString
    {
        ICustomControl Attributes(object htmlAttributes);
        ICustomControl RouteValues(object routeValues);
    }

}
