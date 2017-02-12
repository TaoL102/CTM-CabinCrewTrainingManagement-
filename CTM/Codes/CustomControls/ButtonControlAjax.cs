using System;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CTM.Codes.Extensions;
using CTM.Codes.Helpers;

namespace CTM.Codes.CustomControls
{
    public class ButtonControlAjax : ButtonControl
    {
        private readonly AjaxHelper _ajaxHelper;
        private readonly string _actionName;
        private readonly string _controllerName;
        private string loadingElementId;
        private string updateTargetId;

        public ButtonControlAjax(AjaxHelper ajaxHelper, string actionName, string controllerName)
        {
            _ajaxHelper = ajaxHelper;
            _actionName = actionName;
            _controllerName = controllerName;
        }

        protected override string Render()
        {
            // ajax options
            var ajaxOptions = new AjaxOptions
            {
                HttpMethod = "POST",
                InsertionMode = InsertionMode.Replace,
                UpdateTargetId = updateTargetId,
                LoadingElementId = loadingElementId
            };


            if (_isLinkBtn)
            {

                string innerHtmlOrText = string.Empty;
                if (!string.IsNullOrEmpty(_materialIconName))
                {
                    innerHtmlOrText = string.Format(base.RenderMaterialIcon(_materialIconName));
                }
                else
                {
                    innerHtmlOrText = _btnText ?? string.Empty;
                }

                // Reference:http://stackoverflow.com/questions/12008899/create-ajax-actionlink-with-html-elements-in-the-link-text
                var replacedText = Guid.NewGuid().ToString();
                var actionLink = _ajaxHelper.ActionLink(replacedText, _actionName, _controllerName, _routeValues,
                    ajaxOptions, _htmlAttributes);
                return actionLink.ToString().Replace(replacedText, innerHtmlOrText);
            }
            else
            {
                var form = _ajaxHelper.BeginForm(_actionName, _controllerName, _routeValues, ajaxOptions,
                    _htmlAttributes);
                var attributes = HtmlHelperExtension.ConvertHtmlAttributesToIDictionary(_htmlAttributes);
                // attributes.Add("id", id);
                var button = MvcHtmlString.Create(
                    new ButtonControl().BtnText(_btnText)
                        .MaterialIcon(_materialIconName)
                        .Attributes(_htmlAttributes).ToHtmlString());


                return form.ToString() + button.ToHtmlString() + "</form>";


                // Create tag builder
                TagBuilder builder;
                if (_isLinkBtn)
                {
                    builder = new TagBuilder("a");
                    _isSubmit = false;
                }
                else
                {
                    builder = new TagBuilder("button");
                }

                // attributes
                if (_htmlAttributes.ContainsKey("id"))
                {
                    if (_htmlAttributes["id"] != null)
                    {
                        builder.GenerateId(_htmlAttributes["id"].ToString());
                    }
                    _htmlAttributes.Remove("id");
                }
                if (!string.IsNullOrEmpty(_btnText))
                {
                    _htmlAttributes.Add("value", _btnText);
                }
                if (_isSubmit)
                {
                    _htmlAttributes.Add("type", "submit");
                }
                builder.MergeAttributes(_htmlAttributes);

                // Material Icon
                if (!string.IsNullOrEmpty(_materialIconName))
                {
                    builder.InnerHtml = base.RenderMaterialIcon(_materialIconName);
                }
                // Style
                builder.AddCssClass("btn");
                builder.AddCssClass("btn-" + _buttonStyle.ToString().ToLower());

                return builder.ToString();
            }
        }
    }
}