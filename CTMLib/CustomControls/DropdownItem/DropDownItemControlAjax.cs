using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using CTMLib.CustomControls.Button;
using CTMLib.Extensions;
using CTMLib.Helpers;
using CTMLib.Models;

namespace CTMLib.CustomControls.DropdownItem
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
