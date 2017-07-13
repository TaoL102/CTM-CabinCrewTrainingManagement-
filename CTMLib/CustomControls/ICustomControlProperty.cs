using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CTMLib.Extensions;
using System.Collections.Generic;
using System.Web.Routing;
using System;

namespace CTMLib.CustomControls
{
    public interface ICustomControlBaseProperty : IHtmlString
    {
        string Id { get; set; }
        Dictionary<string, object> HtmlAttributes { get; set; }
        RouteValueDictionary RouteValues { get; set; }
    }

    public interface ISizeProperty : ICustomControlBaseProperty
    {
        SizeOptions Size { get; set; }
    }

    public interface IColorProperty : ICustomControlBaseProperty
    {
        ColorOptions BackgroundColor { get; set; }
    }

    public interface IDialogueProperty : ICustomControlBaseProperty
    {
        bool HasCloseBtn { get; set; }
    }

    public interface IModalProperty : ICustomControlBaseProperty
    {
        string BodyInnerHtml { get; set; }
        string BodyId { get; set; }
        string FooterInnerHtml { get; set; }
    }

    public interface IAjaxProperty : ICustomControlBaseProperty
    {
        string UpdateTargetId { get; set; }
        string LoadingElementId { get; set; }
        string OnSuccessFun { get; set; }
        bool IsPost { get; set; }
    }

    public interface ITextBoxProperty : ICustomControlBaseProperty
    {
        string LabelText { get; set; }
        string Placeholder { get; set; }
        string GoogleIcon { get; set; }
        string GlyphIcon { get; set; }
    }
}
