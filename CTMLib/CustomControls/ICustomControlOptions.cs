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

}
