using System.Collections.Generic;
using System.Web.Mvc;
using CTM.Areas.Search.ViewModels.EnglishTests;
using CTMLib.CustomControls.Pagination;
using CTMLib.Extensions;
using CTMLib.Models;

namespace CTM.Codes.CustomControls.EnglishTests
{
    public static class PaginationExtension
    {
        public static PaginationControlAjax Pagination(this AjaxHelper<IEnumerable<SearchResult>> helper,
            Search searchViewModel, Pager pager)
        {
            return GetPaginationControl(helper, searchViewModel, pager);
        }
        public static PaginationControlAjax Pagination(this AjaxHelper<IEnumerable<SearchResultIsLatest>> helper,
    Search searchViewModel, Pager pager)
        {
            return GetPaginationControl(helper, searchViewModel, pager);
        }

        private static PaginationControlAjax GetPaginationControl(AjaxHelper helper, Search searchViewModel, Pager pager)
        {
            return helper.Pagination("Search", "EnglishTests", "Search", pager)
    .SetUpdateTargetId("full_size_modal_content")
    .SetRouteValues(new
    {
        searchViewModel.IsLatest,
        searchViewModel.CCName,
        searchViewModel.CategoryID,
        searchViewModel.FromDate,
        searchViewModel.ToDate,
        searchViewModel.UploadRecordID
    });
        }
    }
}