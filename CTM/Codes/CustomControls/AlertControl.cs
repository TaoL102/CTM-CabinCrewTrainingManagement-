using System.Web.Mvc;

namespace CTM.Codes.CustomControls
{
    /// <summary>
    /// Reference: https://www.simple-talk.com/dotnet/asp-net/writing-custom-html-helpers-for-asp-net-mvc/
    /// </summary>
    public class AlertControl : IAlertControl
    {
        private readonly string _textOrHtml;
        private AlertStyle _alertStyle;
        private bool _hideCloseButton;
        private object _htmlAttributes;

        public AlertControl(string textOrHtml)
        {
            this._textOrHtml = textOrHtml;
        }

        //Render HTML
        public override string ToString()
        {
            return Render();
        }

        //Return ToString
        public string ToHtmlString()
        {
            return ToString();
        }

        private string Render()
        {
            
            var wrapper = new TagBuilder("div");
            wrapper.AddCssClass("alert");
            wrapper.MergeAttribute("role","alert");
            if (_alertStyle != AlertStyle.Default)
                wrapper.AddCssClass("alert-"+_alertStyle.ToString().ToLower());

            wrapper.InnerHtml = _textOrHtml;

            //Add close button
            if (!_hideCloseButton)
                wrapper.InnerHtml += RenderCloseButton();

            wrapper.MergeAttributes(_htmlAttributes != null ? HtmlHelper.AnonymousObjectToHtmlAttributes(_htmlAttributes) : null);

            

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

        #region FluentAPI

        /// <summary>
        /// Sets the display style to Success
        /// </summary>
        public IAlertControlFluentOptions Success()
        {
            _alertStyle = AlertStyle.Success;
            return new AlertControlFluentOptions(this);
        }

        /// <summary>
        /// Sets the display style to Warning
        /// </summary>
        /// <returns></returns>
        public IAlertControlFluentOptions Warning()
        {
            _alertStyle = AlertStyle.Warning;
            return new AlertControlFluentOptions(this);
        }

        /// <summary>
        /// Sets the display style to Info
        /// </summary>
        /// <returns></returns>
        public IAlertControlFluentOptions Info()
        {
            _alertStyle = AlertStyle.Info;
            return new AlertControlFluentOptions(this);
        }

        public IAlertControlFluentOptions Danger()
        {
            _alertStyle = AlertStyle.Danger;
            return new AlertControlFluentOptions(this);
        }

        /// <summary>
        /// Sets the close button visibility
        /// </summary>
        /// <returns></returns>
        public IAlertControlFluentOptions HideCloseButton(bool hideCloseButton = true)
        {
            this._hideCloseButton = hideCloseButton;
            return new AlertControlFluentOptions(this);
        }

        /// <summary>
        /// An object that contains the HTML attributes to set for the element.
        /// </summary>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public ICustomControl Attributes(object htmlAttributes)
        {
            this._htmlAttributes = htmlAttributes;
            return new AlertControlFluentOptions(this);
        }

        public ICustomControl RouteValues(object routeValues)
        {
            return new AlertControlFluentOptions(this);
        }

        #endregion //FluentAPI 

        public enum AlertStyle
        {
            Default, Success, Info, Warning, Danger,
        }

    }


    public interface IAlertControl : IAlertControlFluentOptions
    {
        IAlertControlFluentOptions Success();
        IAlertControlFluentOptions Warning();
        IAlertControlFluentOptions Info();
        IAlertControlFluentOptions Danger();
    }

    public interface IAlertControlFluentOptions : ICustomControl
    {
        IAlertControlFluentOptions HideCloseButton(bool hideCloseButton = true);
    }

    public class AlertControlFluentOptions : IAlertControlFluentOptions
    {
        private readonly AlertControl _parent;

        public AlertControlFluentOptions(AlertControl parent)
        {
            this._parent = parent;
        }

        public IAlertControlFluentOptions HideCloseButton(bool hideCloseButton = true)
        {
            return _parent.HideCloseButton(hideCloseButton);
        }

        public ICustomControl Attributes(object htmlAttributes)
        {
            return _parent.Attributes(htmlAttributes);
        }

        public ICustomControl RouteValues(object routeValues)
        {
            return _parent.RouteValues(routeValues);
        }

        public override string ToString()
        {
            return _parent.ToString();
        }

        public string ToHtmlString()
        {
            return _parent.ToHtmlString();
        }


    }


}