using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace CTM.App_Code
{
    /// <summary>
    /// To solve 
    /// Reference: http://stackoverflow.com/questions/4710853/using-mvc-htmlhelper-extensions-from-razor-declarative-views
    /// </summary>
    public class HelperPage : System.Web.WebPages.HelperPage
    {
        public static new HtmlHelper Html
        {
            get { return ((System.Web.Mvc.WebViewPage)WebPageContext.Current.Page).Html; }
        }

        public static new AjaxHelper Ajax
        {
            get { return ((System.Web.Mvc.WebViewPage)WebPageContext.Current.Page).Ajax; }
        }

        public static new UrlHelper Url
        {
            get { return ((System.Web.Mvc.WebViewPage)WebPageContext.Current.Page).Url; }
        }

    }

    public class HelperPage<TModel> : System.Web.WebPages.HelperPage
    {

        public static new HtmlHelper<TModel> Html
        {
            get { return ((System.Web.Mvc.WebViewPage<TModel>)WebPageContext.Current.Page).Html; }
        }

        public static new AjaxHelper<TModel> Ajax
        {
            get { return ((System.Web.Mvc.WebViewPage<TModel>)WebPageContext.Current.Page).Ajax; }
        }
        public static new UrlHelper Url
        {
            get { return ((System.Web.Mvc.WebViewPage)WebPageContext.Current.Page).Url; }
        }

    }

}