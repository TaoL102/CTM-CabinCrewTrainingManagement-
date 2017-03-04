using System.Web.Mvc;
using CTMLib.Extensions;
using CTMLib.Models;

namespace CTMLib.CustomControls.Pagination
{
    public class PaginationControlAjax : CustomControlBase, IAjaxOptions
    {
        private readonly AjaxHelper _ajaxHelper;
        private readonly string _controllerName;
        private readonly string _areaName;
        private readonly Pager _pager;
        private readonly string _actionName;

        public string UpdateTargetId { get; set; }
        public string LoadingElementId { get; set; }
        public string OnSuccessFun { get; set; }
        public bool IsPost { get; set; }

        public PaginationControlAjax(AjaxHelper ajaxHelper, string actionName, string controllerName, string areaName, Pager pager)
        {
            _ajaxHelper = ajaxHelper;
            _controllerName = controllerName;
            _areaName = areaName;
            _pager = pager;
            _actionName = actionName;
        }

        protected override string Render()
        {
            // Wrapper
            TagBuilder nav=new TagBuilder("nav");
            TagBuilder ul = new TagBuilder("ul");
            ul.AddCssClass("pagination");

            string first = null;
            string previous = null;
            string pages = null;
            string next = null;
            string last = null;

            // First & Previews
            if (_pager.CurrentPage > 1)
            {
                first = LiWithLink("First", GetRouteValuesByPage(1));
                previous = LiWithLink("Previews", GetRouteValuesByPage(_pager.CurrentPage - 1));
            }
            for (var page = _pager.StartPage; page <= _pager.EndPage; page++)
            {
                var liCssClass = page == _pager.CurrentPage ? "active" : "";
                pages += LiWithLink(page.ToString(), GetRouteValuesByPage(page), liCssClass);
            }
            if (_pager.CurrentPage < _pager.TotalPages)
            {
                next = LiWithLink("Next", GetRouteValuesByPage(_pager.CurrentPage + 1));
                last = LiWithLink("Last", GetRouteValuesByPage(_pager.TotalPages));
            }

            ul.InnerHtml = first + previous + pages + next + last;
            nav.InnerHtml = ul.ToString();
            return nav.ToString();
        }

        private object GetRouteValuesByPage(int page)
        {
            return
            HtmlHelperExtension.AddRouteValue(RouteValues, new {Page = page});
        }

        private string LiWithLink(string text,object routeValues,string liCssClass = null)
        {
            // Wrapper
            TagBuilder li = new TagBuilder("li");
            li.AddCssClass(liCssClass);
            li.AddCssClass("page-item");

            // link
            var a = _ajaxHelper.
                Button(_actionName, _controllerName, _areaName)
                .SetText(text)
                .IsLinkBtn(true)
                .SetRouteValues(routeValues)
                .SetUpdateTargetId(UpdateTargetId)
                .RemoveAllCssClass()
                .AddCssClass("page-link");

            li.InnerHtml = a.ToString();
            return li.ToString();
        }


    }
}
