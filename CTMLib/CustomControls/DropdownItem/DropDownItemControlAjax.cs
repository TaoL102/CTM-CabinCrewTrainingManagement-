using System.Web.Mvc;
using CTMCustomControlLib.CustomControls.Button;
using CTMCustomControlLib.Helpers;
using CTMCustomControlLib.Extensions;

namespace CTMCustomControlLib.CustomControls.DropdownItem
{
    public class DropDownItemControlAjax : ButtonControlAjax
    {
        public DropDownItemControlAjax(AjaxHelper ajaxHelper, string actionName, string controllerName, string areaName) : base(ajaxHelper, actionName, controllerName, areaName)
        {
            this.IsLinkBtn = true;
            this.AddCssClass(ConstantHelper.CssDropDownList);
        }
    }
}
