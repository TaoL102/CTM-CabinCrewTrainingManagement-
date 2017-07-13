using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using CTMLib.CustomControls.Button;
using CTMLib.Extensions;
using CTMLib.Helpers;

namespace CTMLib.CustomControls.DropdownItem
{
    public class DropDownItemControl:ButtonControl
    {
        private readonly HtmlHelper _htmlHelper;
        private readonly string _actionName;
        private readonly string _controllerName;
        private readonly string _areaName;

        public DropDownItemControl(HtmlHelper htmlHelper, string actionName, string controllerName, string areaName)
        {
            _htmlHelper = htmlHelper;
            _actionName = actionName;
            _controllerName = controllerName;
            _areaName = areaName;

            // Basic Style
            HtmlAttributes = HtmlHelperExtension.AddCssClass(HtmlAttributes, CssHelper<ButtonControl>.ControlTypeAbbr);
        }

        protected override string Render()
        {
            // CSS: SetColor & SetSize
            this.AddCssClass(CssHelper<ButtonControl>.ControlTypeAbbr);
            this.AddCssClass(CssHelper<ButtonControl>.ConvertToCss(BackgroundColor));
            this.AddCssClass(CssHelper<ButtonControl>.ConvertToCss(Size));
            this.AddCssClass(ConstantHelper.CssDropDownList);

            // Route values
            RouteValues = HtmlHelperExtension.AddRouteValue(RouteValues, new { area = _areaName });

            String actionLinkStr = _htmlHelper.ActionLink(Text, _actionName, _controllerName, RouteValues,HtmlAttributes).ToString();

            return actionLinkStr;
        }
    }
}
