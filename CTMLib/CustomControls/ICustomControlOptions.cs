using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CTMLib.Extensions;
using System.Collections.Generic;
using System.Web.Routing;
using System;

namespace CTMLib.CustomControls
{
    public interface ICustomControlOptions : IHtmlString
    {
        string Id { get; set; }
        Dictionary<string, object> HtmlAttributes { get; set; }
        RouteValueDictionary RouteValues { get; set; }
    }

    public interface ISizeOptions : ICustomControlOptions
    {
        SizeOptions Size { get; set; }
    }

    public interface IColorOptions : ICustomControlOptions
    {
        ColorOptions BackgroundColor { get; set; }
    }

    public interface IDialogueOptions : ICustomControlOptions
    {
        bool HasCloseBtn { get; set; }
    }

    public interface IModalOptions : ICustomControlOptions
    {
        string BodyInnerHtml { get; set; }
        string BodyId { get; set; }
        string FooterInnerHtml { get; set; }
    }

    public interface IAjaxOptions : ICustomControlOptions
    {
        string UpdateTargetId { get; set; }
        string LoadingElementId { get; set; }
        string OnSuccessFun { get; set; }
        bool IsPost { get; set; }
    }

    public interface ITextBoxOptions : ICustomControlOptions
    {
        string LabelText { get; set; }
        string Placeholder { get; set; }
        string GoogleIcon { get; set; }
        string GlyphIcon { get; set; }
    }
}
