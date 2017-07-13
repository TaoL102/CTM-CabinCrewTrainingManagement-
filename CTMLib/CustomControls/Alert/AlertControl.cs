using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using CTMLib.Extensions;
using CTMLib.Helpers;

namespace CTMLib.CustomControls.Alert
{

    public class AlertControl : CustomControlBase, IDialogueProperty,IColorProperty
    {
        private readonly string _textOrHtml;
        public bool HasCloseBtn { get; set; }
        public ColorOptions BackgroundColor { get; set; }

        #region Constructor

        public AlertControl(string textOrHtml)
        {
            this._textOrHtml = textOrHtml;
        }

        #endregion


        #region Overriden Methods
        protected override string Render()
        {

            var wrapper = new TagBuilder("div");
            // Id
            wrapper.GenerateId(Id);

            // HTML Attributes
            wrapper.MergeAttributes(HtmlAttributes);

            // Class
            wrapper.AddCssClass("alert");
            wrapper.MergeAttribute("role", "alert");
            wrapper.AddCssClass(CssHelper<AlertControl>.ConvertToCss(BackgroundColor));

            // Inner Html
            wrapper.InnerHtml = _textOrHtml;

            // Close button
            if (HasCloseBtn)
            {
                wrapper.InnerHtml += RenderCloseButton();
            }

            return wrapper.ToString();
        }

        #endregion

        #region Private Methods

        private static TagBuilder RenderCloseButton()
        {
            //<a href="" class="close">x</a>
            var closeButton = new TagBuilder("a");
            closeButton.AddCssClass("close");
            closeButton.Attributes.Add("href", "");
            closeButton.InnerHtml = "×";
            return closeButton;
        }


        #endregion
    }
}