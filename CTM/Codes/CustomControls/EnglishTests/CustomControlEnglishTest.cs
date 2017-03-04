using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using CTM.Areas.Search.ViewModels;
using CTM.Areas.Search.ViewModels.EnglishTests;
using CTM.Codes.CustomControls.Shared;
using CTMLib.CustomControls.Div;
using CTMLib.Extensions;
using CTMLib.Helpers;
using CTMLib.Models;

namespace CTM.Codes.CustomControls.EnglishTests
{
    public class CustomControlEnglishTest : CustomControlBase<EnglishTest>
    {

        protected sealed override string GenerateSearchFormBody(AjaxHelper helper)
        {
            var htmlHelper = new HtmlHelper<Search>(helper.ViewContext, helper.ViewDataContainer);
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext, helper.RouteCollection);
            var model = (Search)helper.ViewData.Model;

            string hidableDivId = Guid.NewGuid().ToString();
            var ccName = htmlHelper
                .TextBoxGroupFor(o => o.CCName, new { @class = "col-sm-12" }, new
                {
                    data_url = urlHelper.Action("GetCabinCrewNames", "Query", new { area = "API" })
                });
            var categoryID = htmlHelper
                .DropDownListGroupFor(o => o.CategoryID, model.CateforyList, new { @class = "col-sm-4" });
            var fromDate = htmlHelper
                .DateTimeGroupFor(o => o.FromDate, new { @class = "col-sm-4" });
            var toDate = htmlHelper
                .DateTimeGroupFor(o => o.ToDate, new { @class = "col-sm-4" });
            var isLatest = htmlHelper
                .CheckBoxGroupFor(o => o.IsLatest, new { @class = "col-12" }, new { data_hidableDivId = hidableDivId });

            var searchBtn = htmlHelper.Button()
                .IsSubmitBtn(true)
                .SetMaterialIcon("search")
                .AddCssClass("col-sm-6");
            var downloadBtn = htmlHelper.Button()
                .SetMaterialIcon("file_download")
                .AddCssClass("col-sm-6")
                .MergeAttribute("onclick", "this.form.submit(); ");
            var buttonWrapper = new DivControl(searchBtn.ToHtmlString() + downloadBtn)
                .AddCssClass("col-12");

            // Wrap
            var row1 = new DivControl(ccName.ToHtmlString()).AddCssClass("row");
            var row3 = new DivControl(isLatest.ToHtmlString()).AddCssClass("row");
            var row2 = new DivControl(categoryID.ToHtmlString()
                                      + fromDate
                                      + toDate)
                .AddCssClass("row")
                .MergeAttribute("id", hidableDivId)
                .Hide();
            var row4 = new DivControl(buttonWrapper.ToString()).AddCssClass("row");

            return row1.ToHtmlString()
                   + row2
                   + row3
                   + row4;
        }

        protected override object GetPaginationRouteValues(ISearchViewModel viewModel)
        {
            var searchViewModel = (Search)viewModel;
            return new
            {
                searchViewModel.IsLatest,
                searchViewModel.CCName,
                searchViewModel.CategoryID,
                searchViewModel.FromDate,
                searchViewModel.ToDate,
                searchViewModel.UploadRecordID
            };
        }

        protected override Dictionary<string, string[]> GetSearchResultRowsWithId<TModel>(HtmlHelper<IEnumerable<TModel>> helper, IEnumerable<TModel> models)
        {
            var ajaxHelper = new AjaxHelper<IEnumerable<SearchResult>>(
    helper.ViewContext, helper.ViewDataContainer);

            var rowsWithId = new Dictionary<string, string[]>();
            foreach (var item in (IEnumerable<SearchResult>)models)
            {
                rowsWithId.Add(item.ID, new string[]
                {
                    helper.DisplayValueFor(o=>item.CabinCrewName).ToString(),
                    helper.DisplayValueFor(o=>item.Type).ToString(),
                    helper.DisplayValueFor(o=>item.Grade).ToString() ,
                    helper.DisplayValueFor(o=>item.CategoryName).ToString(),
                    helper.DisplayValueFor(o=>item.Date).ToString(),
                    ajaxHelper.Button_Edit(item.ID).AddCssClass("btn-round").ToHtmlString(),
                    ajaxHelper.Button_Delete(item.ID).AddCssClass("btn-round").ToHtmlString(),
                });
            }
            return rowsWithId;;
        }

        protected override string[] GetSearchResultHeader<TModel>(HtmlHelper<IEnumerable<TModel>> helper)
        {
            var htmlHelper = new HtmlHelper<IEnumerable<SearchResult>>(
    helper.ViewContext, helper.ViewDataContainer);
            
            var header = new string[]
           {
                htmlHelper.DisplayNameFor(o => o.CabinCrewName).ToString(),
                htmlHelper.DisplayNameFor(o => o.Type).ToString(),
                htmlHelper.DisplayNameFor(o => o.Grade).ToString(),
                htmlHelper.DisplayNameFor(o => o.CategoryName).ToString(),
                htmlHelper.DisplayNameFor(o => o.Date).ToString(),
                "",""
           };

            return header;;
        }

        protected override string[] GetSearchResultHeader_IsLatest<TModel>(HtmlHelper<IEnumerable<TModel>> helper)
        {
            var htmlHelper = new HtmlHelper<IEnumerable<SearchResultIsLatest>>(
helper.ViewContext, helper.ViewDataContainer);
            var header = new string[]
            {
                htmlHelper.DisplayNameFor(o => o.CabinCrewName).ToString(),
                LocalizationHelper.GetModelString("CabinAnnoucement"),
                "","",
                LocalizationHelper.GetModelString("SpokenSkill"),
                 "","",
            };
            return header;
        }

        protected override Dictionary<string, string[]> GetSearchResultRowsWithId_IsLatest<TModel>(HtmlHelper<IEnumerable<TModel>> helper, IEnumerable<TModel> models)
        {
            var modelsList = (IList<SearchResultIsLatest>)models.ToList();
            var rows = new Dictionary<string, string[]>();
            for (int i = 0; i < modelsList.Count(); i++)
            {
                var item = modelsList[i];
                rows.Add(i.ToString(), new string[]
                 {
                    helper.DisplayValueFor(o=>item.CabinCrewName).ToString(),
                    helper.DisplayValueFor(o=>item.CabinAnnoucementGrade).ToString(),
                    helper.DisplayValueFor(o=>item.CabinAnnoucementCategoryName).ToString() ,
                    helper.DisplayValueFor(o=>item.CabinAnnoucementDate).ToString(),
                    helper.DisplayValueFor(o=>item.SpokenSkillGrade).ToString(),
                    helper.DisplayValueFor(o=>item.SpokenSkillCategoryName).ToString(),
                    helper.DisplayValueFor(o=>item.SpokenSkillDate).ToString(),
                 });
            }
            return rows;
        }
    }

}