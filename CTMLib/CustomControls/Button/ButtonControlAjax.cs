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

        public string UpdateTargetId { get; set; }
        public string LoadingElementId { get; set; }
        public string OnSuccessFun { get; set; }
        public bool IsPost { get; set; }

        public ButtonControlAjax(AjaxHelper ajaxHelper, string actionName, string controllerName)
        {
            _ajaxHelper = ajaxHelper;
            _actionName = actionName;
            _controllerName = controllerName;
        }

        protected override string Render()
        {
            // Data
            var ajaxOptions = new AjaxOptions
            {
                HttpMethod = (IsPost?"POST":"GET"),
                InsertionMode = InsertionMode.Replace,
                UpdateTargetId = UpdateTargetId,
                LoadingElementId = LoadingElementId,
                OnSuccess = OnSuccessFun
            };

            // Id
            if (Id!=null)
            {
                HtmlAttributes.Add("id", Id);
            }

            // Style
            HtmlAttributes =HtmlHelperExtension.AddCssClass(HtmlAttributes, CssHelper<ButtonControlAjax>.ControlTypeAbbr);
            HtmlAttributes = HtmlHelperExtension.AddCssClass(HtmlAttributes, CssHelper<ButtonControlAjax>.ConvertToCss(BackgroundColor));
            HtmlAttributes = HtmlHelperExtension.AddCssClass(HtmlAttributes, CssHelper<ButtonControlAjax>.ConvertToCss(Size));

            if (IsLinkBtn)
            {

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
            else
            {
                var form = _ajaxHelper.BeginForm(_actionName, _controllerName, RouteValues, ajaxOptions,
                    HtmlAttributes);
                var attributes = HtmlHelperExtension.ConvertHtmlAttributesToIDictionary(HtmlAttributes);
               
                var button =
                    new ButtonControl().SetText(Text)
                        .SetMaterialIcon(MaterialIcon)
                        .SetAttributes(attributes)
                        .SetRouteValues(RouteValues)
                        .IsSubmitBtn(true);
                _ajaxHelper.ViewContext.Writer.Write(button);
                form.EndForm();

                var htmlStr = form.ToString().Replace("System.Web.Mvc.Html.MvcForm","");

                return htmlStr;
            }
        }

    }
}