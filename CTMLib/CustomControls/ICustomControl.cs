using System.Web;

namespace CTMLib.CustomControls
{
    public interface ICustomControl : IHtmlString
    {
        ICustomControl Attributes(object htmlAttributes);
        ICustomControl RouteValues(object routeValues);
    }

}
