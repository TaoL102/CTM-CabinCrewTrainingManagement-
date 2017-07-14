using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using CTM.Areas.ManageData.ViewModels.EnglishTests;
using CTM.Areas.Search.ViewModels;
using CTM.Areas.Search.ViewModels.EnglishTests;
using CTM.Codes.CustomControls.Shared;
using CTM.Codes.Extensions;
using CTM.Models;
using CTMCustomControlLib.CustomControls.Div;
using CTMCustomControlLib.Extensions;

namespace CTM.Codes.CustomControls.EnglishTests
{
    public class CustomControlEnglishTest : CustomControlBase<EnglishTest>
    {
        protected override object PaginationRouteValues(ISearchViewModel viewModel)
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

        protected sealed override string Form_Search_Body(AjaxHelper helper)
        {
            var htmlHelper = new HtmlHelper<Search>(helper.ViewContext, helper.ViewDataContainer);
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext, helper.RouteCollection);
            var model = (Search)helper.ViewData.Model;

            string hidableDivId = Guid.NewGuid().ToString();
            var ccName = htmlHelper
                .TextBoxGroupFor(o => o.CCName, new { @class = "col-sm-12" }, new
                {
                    data_url = urlHelper.Action("GetCabinCrewNames", "Query", new { area = Helpers.ConstantHelper.AreaNameAPI }),
                    data_allowMultipleValues = true
                });
            var categoryID = htmlHelper
                .DropDownListGroupFor(o => o.CategoryID, model.CategoryList, new { @class = "col-sm-4" });
            var fromDate = htmlHelper
                .DateTimeGroupFor(o => o.FromDate, new { @class = "col-sm-4" });
            var toDate = htmlHelper
                .DateTimeGroupFor(o => o.ToDate, new { @class = "col-sm-4" });
            var isLatest = htmlHelper
                .CheckBoxGroupFor(o => o.IsLatest, new { @class = "col-12" }, new { data_hidableDivId = hidableDivId });



            // Wrap
            var row1 = new DivControl(ccName.ToHtmlString()).AddCssClass("row");
            var row3 = new DivControl(isLatest.ToHtmlString()).AddCssClass("row");
            var row2 = new DivControl(categoryID.ToHtmlString()
                                      + fromDate
                                      + toDate)
                .AddCssClass("row")
                .MergeAttribute("id", hidableDivId)
                .Hide();
           
            return row1.ToHtmlString()
                   + row2
                   + row3;
        }
        
        protected override string Form_Create_Body(AjaxHelper helper)
        {
            var htmlHelper = new HtmlHelper<Create>(helper.ViewContext, helper.ViewDataContainer);
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext, helper.RouteCollection);
            var model = htmlHelper.ViewData.Model;

            var row1 = htmlHelper.TextBoxGroupFor(o => o.CCName, null, new
            {
                data_url = urlHelper.Action("GetCabinCrewNames", "Query", new { area = Helpers.ConstantHelper.AreaNameAPI }),
            });

            var row2 = htmlHelper.DropDownListGroupFor(o => o.CategoryID, model.CategoryList);

            var row3 = htmlHelper.EnumDropDownListGroupFor(o => o.Type);

            var row4 = htmlHelper.TextBoxGroupFor(o => o.Grade);

            var row5 = htmlHelper.DateTimeGroupFor(o => o.Date);

            return string.Concat(row1, row2, row3, row4, row5);
        }

        protected override string Form_Upload_Body(AjaxHelper helper)
        {
            var htmlHelper = new HtmlHelper<Upload>(helper.ViewContext, helper.ViewDataContainer);

            var model = htmlHelper.ViewData.Model;

            var row1 = htmlHelper.DropDownListGroupFor(o => o.CategoryID, model.CategoryList);

            var row2 = htmlHelper.DateTimeGroupFor(o => o.Date);

            var row3 = htmlHelper.FileGroupFor(o => o.File);

            return string.Concat(row1, row2, row3);

        }

        protected override string Form_Edit_Body(AjaxHelper helper)
        {
            var htmlHelper = new HtmlHelper<EnglishTest>(helper.ViewContext, helper.ViewDataContainer);
            var model = (EnglishTest)helper.ViewData.Model;
            var row1 = new TagBuilder("h4")
            {
                InnerHtml =
                    htmlHelper.DisplayValueFor(o => o.CabinCrew.Name) +
                    "(" +
                    htmlHelper.DisplayValueFor(o => o.Type) +
                    ")",
            };

            var row2 =
                   htmlHelper.HiddenFor(o => o.ID).ToString() +
                   htmlHelper.HiddenFor(o => o.CabinCrewID) +
                   htmlHelper.HiddenFor(o => o.UploadRecordID) +
                   htmlHelper.HiddenFor(o => o.Type);


            var row3 = htmlHelper.TextBoxGroupFor(o => o.Grade);
            var row4 = htmlHelper.DropDownListGroupFor(o => o.CategoryID, null);
            var row5 = htmlHelper.DateTimeGroupFor(o => o.Date);

            return string.Concat(row1, row2, row3, row4, row5);
        }


    }

}