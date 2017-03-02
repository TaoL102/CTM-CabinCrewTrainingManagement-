using System;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CTMLib.Extensions;
using CTMLib.Helpers;

namespace CTMLib.CustomControls.Button
{
    public class ButtonControlAjax : ButtonControlBase, IAjaxOptions
    {
        private readonly AjaxHelper _ajaxHelper;
        private readonly string _actionName;
        private readonly string _controllerName;
        private readonly string _areaName;

        public string UpdateTargetId { get; set; }
        public string LoadingElementId { get; set; }
        public string OnSuccessFun { get; set; }
        public bool IsPost { get; set; }

        public ButtonControlAjax(AjaxHelper ajaxHelper, string actionName, string controllerName, string areaName)
        {
            _ajaxHelper = ajaxHelper;
            _actionName = actionName;
            _controllerName = controllerName;
            _areaName = areaName;

            // Basic Style
            HtmlAttributes = HtmlHelperExtension.AddCssClass(HtmlAttributes, CssHelper<ButtonControlAjax>.ControlTypeAbbr);
        }

        protected override string Render()
        {
            // Data
            var ajaxOptions = new AjaxOptions
            {
                HttpMethod = (IsPost ? "POST" : "GET"),
                InsertionMode = InsertionMode.Replace,
                UpdateTargetId = UpdateTargetId,
                LoadingElementId = LoadingElementId,
                OnSuccess = "new function(){"+OnSuccessFun?.Replace("\"", "\'") +"}"
            };

            RouteValues = HtmlHelperExtension.AddRouteValue(RouteValues, new { area = _areaName });

            // Style
            HtmlAttributes = HtmlHelperExtension.AddCssClass(HtmlAttributes, CssHelper<ButtonControlAjax>.ConvertToCss(BackgroundColor));
            HtmlAttributes = HtmlHelperExtension.AddCssClass(HtmlAttributes, CssHelper<ButtonControlAjax>.ConvertToCss(Size));

            // Id
            if (Id != null)
            {
                HtmlAttributes.Add("id", Id);
            }

            string innerHtmlOrText = string.Empty;
            if (!string.IsNullOrEmpty(MaterialIcon))
            {
                innerHtmlOrText = string.Format(base.RenderMaterialIcon(MaterialIcon));
            }
            else
            {
                innerHtmlOrText = Text ?? string.Empty;
            }

            // Reference:http://stackoverflow.com/questions/12008899/create-ajax-actionlink-with-html-elements-in-the-link-text
            var replacedText = Guid.NewGuid().ToString();
            var actionLink = _ajaxHelper.ActionLink(replacedText, _actionName, _controllerName, RouteValues,
                ajaxOptions, HtmlAttributes);
            return actionLink.ToString().Replace(replacedText, innerHtmlOrText);
        }

    }
}