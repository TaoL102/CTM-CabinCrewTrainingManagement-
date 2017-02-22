using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace CTMLib.CustomControls.Alert
{

    public class AlertControl : CustomControlBase, IDialogueOptions
    {
        private readonly string _textOrHtml;
        public bool HasCloseBtn { get; set; }
        public ColorOptions Color { get; set; }

        #region Constructor

        public AlertControl(string textOrHtml)
        {
            this._textOrHtml = textOrHtml;
        }

        #endregion


        #region Private Methods

        protected override string Render()
        {
            
            var wrapper = new TagBuilder("div");
            wrapper.AddCssClass("alert");
            wrapper.MergeAttribute("role","alert");
            if (Color != ColorOptions.Default)
                wrapper.AddCssClass("alert-"+ Color.ToString().ToLower());

            wrapper.InnerHtml = _textOrHtml;

            //Add close button
            if (HasCloseBtn)
                wrapper.InnerHtml += RenderCloseButton();

            wrapper.MergeAttributes(HtmlAttributes != null ? HtmlHelper.AnonymousObjectToHtmlAttributes(HtmlAttributes) : null);

            

            return wrapper.ToString();
        }

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