using System;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CTMLib.Extensions;

namespace CTMLib.CustomControls.Button
{
    public class ButtonControlAjax : ButtonControlBase
    {
        private readonly AjaxHelper _ajaxHelper;
        private readonly string _actionName;
        private readonly string _controllerName;
        private string _loadingElementId;
        private string _updateTargetId;

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
                UpdateTargetId = _updateTargetId,
                LoadingElementId = _loadingElementId
            };


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
                // attributes.Add("id", id);
                var button = MvcHtmlString.Create(
                    new ButtonControl().SetText(Text)
                        .SetMaterialIcon(MaterialIcon)
                        .SetAttributes(HtmlAttributes).ToHtmlString());


                return form.ToString() + button.ToHtmlString() + "</form>";


                // Create tag builder
                TagBuilder builder;
                if (IsLinkBtn)
                {
                    builder = new TagBuilder("a");
                    IsSubmitBtn = false;
                }
                else
                {
                    builder = new TagBuilder("button");
                }

                // attributes
                if (HtmlAttributes.ContainsKey("id"))
                {
                    if (HtmlAttributes["id"] != null)
                    {
                        builder.GenerateId(HtmlAttributes["id"].ToString());
                    }
                    HtmlAttributes.Remove("id");
                }
                if (!string.IsNullOrEmpty(Text))
                {
                    HtmlAttributes.Add("value", Text);
                }
                if (IsSubmitBtn)
                {
                    HtmlAttributes.Add("type", "submit");
                }
                builder.MergeAttributes(HtmlAttributes);

                // Material Icon
                if (!string.IsNullOrEmpty(MaterialIcon))
                {
                    builder.InnerHtml = base.RenderMaterialIcon(MaterialIcon);
                }
                // Style
                builder.AddCssClass("btn");
                builder.AddCssClass("btn-" + BackgroundColor.ToString().ToLower());

                return builder.ToString();
            }
        }
    }
}