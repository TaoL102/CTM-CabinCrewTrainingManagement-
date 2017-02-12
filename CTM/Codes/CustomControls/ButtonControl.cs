using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using CTM.Codes.Extensions;
using CTM.Codes.Helpers;

namespace CTM.Codes.CustomControls
{
    /// <summary>
    /// Reference: https://www.simple-talk.com/dotnet/asp-net/writing-custom-html-helpers-for-asp-net-mvc/
    /// </summary>
    public class ButtonControl : IButtonControl
    {
        protected string _btnText;
        protected ButtonStyle _buttonStyle;
        protected Dictionary<string, object> _htmlAttributes;
        protected RouteValueDictionary _routeValues;
        protected bool _isLinkBtn;
        protected bool _isSubmit;
        protected string _materialIconName;

        public ButtonControl()
        {
            _buttonStyle=ButtonStyle.Default;
            _htmlAttributes=new Dictionary<string, object>();
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

        protected virtual string Render()
        {
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
                if (_htmlAttributes["id"]!=null)
                {
                    builder.GenerateId(_htmlAttributes["id"].ToString());
                }
                _htmlAttributes.Remove("id");
            }
            if (!string.IsNullOrEmpty(_btnText))
            {
                _htmlAttributes.Add("value",_btnText);
            }
            if (_isSubmit)
            {
                _htmlAttributes.Add("type", "submit");
            }
            builder.MergeAttributes(_htmlAttributes);

            // Material Icon
            if (!string.IsNullOrEmpty(_materialIconName))
            {
                builder.InnerHtml = RenderMaterialIcon(_materialIconName);
            }
            // Style
            builder.AddCssClass("btn");
            builder.AddCssClass("btn-" + _buttonStyle.ToString().ToLower());

            return builder.ToString();
        }

        protected string RenderMaterialIcon(string materialIconName)
        {
            var builderI = new TagBuilder("i");
            builderI.AddCssClass("material-icons");
            builderI.InnerHtml = materialIconName;
            return builderI.ToString();
        }

        #region FluentAPI

        /// <summary>
        /// Sets the display style to Success
        /// </summary>
        public IButtonControlFluentOptions Success()
        {
            _buttonStyle = ButtonStyle.Success;
            return new ButtonControlFluentOptions(this);
        }

        /// <summary>
        /// Sets the display style to Warning
        /// </summary>
        /// <returns></returns>
        public IButtonControlFluentOptions Warning()
        {
            _buttonStyle = ButtonStyle.Warning;
            return new ButtonControlFluentOptions(this);
        }

        /// <summary>
        /// Sets the display style to Info
        /// </summary>
        /// <returns></returns>
        public IButtonControlFluentOptions Info()
        {
            _buttonStyle = ButtonStyle.Info;
            return new ButtonControlFluentOptions(this);
        }

        public IButtonControlFluentOptions Danger()
        {
            _buttonStyle = ButtonStyle.Danger;
            return new ButtonControlFluentOptions(this);
        }

        /// <summary>
        /// An object that contains the HTML attributes to set for the element.
        /// </summary>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public ICustomControl Attributes(object htmlAttributes)
        {
            this._htmlAttributes =HtmlHelperExtension.ConvertHtmlAttributesToIDictionary(htmlAttributes);
            return new ButtonControlFluentOptions(this);
        }

        public ICustomControl RouteValues(object routeValues)
        {
            return new ButtonControlFluentOptions(this);
        }


        #endregion //FluentAPI 

        public enum ButtonStyle
        {
            Default, Success, Info, Warning, Danger,
        }

        public IButtonControlFluentOptions BtnText(string text)
        {
            _btnText = text;
            return new ButtonControlFluentOptions(this);
        }

        public IButtonControlFluentOptions LinkBtn(bool isLinkButton)
        {
            _isLinkBtn = isLinkButton;
            return new ButtonControlFluentOptions(this);
        }

        public IButtonControlFluentOptions MaterialIcon(string iconName)
        {
            _materialIconName = iconName;
            return new ButtonControlFluentOptions(this);
        }

        public IButtonControlFluentOptions SubmitBtn(bool isSubmitButton)
        {
            _isSubmit = isSubmitButton;
            return new ButtonControlFluentOptions(this);
        }

        public IButtonControlFluentOptions Small()
        {
            _htmlAttributes = HtmlHelperExtension.AddCssClass(_htmlAttributes, "btn-small");
            return new ButtonControlFluentOptions(this);
        }

        public IButtonControlFluentOptions Large()
        {
            _htmlAttributes = HtmlHelperExtension.AddCssClass(_htmlAttributes, "btn-large");
            return new ButtonControlFluentOptions(this);
        }
    }


    public interface IButtonControl : IButtonControlSizeFluentOptions
    {
        IButtonControlFluentOptions Success();
        IButtonControlFluentOptions Warning();
        IButtonControlFluentOptions Info();
        IButtonControlFluentOptions Danger();
    }

    public interface IButtonControlSizeFluentOptions : IButtonControlFluentOptions
    {
        IButtonControlFluentOptions Small();
        IButtonControlFluentOptions Large();
    }

    public interface IButtonControlFluentOptions : ICustomControl
    {
        IButtonControlFluentOptions BtnText(string text);
        IButtonControlFluentOptions LinkBtn(bool isLinkButton);
        IButtonControlFluentOptions MaterialIcon(string iconName);
        IButtonControlFluentOptions SubmitBtn(bool isSubmitButton);
    }

    public class ButtonControlFluentOptions : IButtonControlFluentOptions
    {
        private readonly ButtonControl _parent;

        public ButtonControlFluentOptions(ButtonControl parent)
        {
            this._parent = parent;
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

        public IButtonControlFluentOptions BtnText(string text)
        {
            return _parent.BtnText(text);
        }

        public IButtonControlFluentOptions LinkBtn(bool isLinkButton)
        {
            return _parent.LinkBtn(isLinkButton);
        }

        public IButtonControlFluentOptions MaterialIcon(string iconName)
        {
            return _parent.MaterialIcon(iconName);
        }

        public IButtonControlFluentOptions SubmitBtn(bool isSubmitButton)
        {
            return _parent.SubmitBtn(isSubmitButton);
        }
    }


}