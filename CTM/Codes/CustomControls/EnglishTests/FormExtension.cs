using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using CTMLib.CustomControls.TextBox;
using CTMLib.Extensions;
using CTMLib.Resources;
using CTM.Areas.Search.ViewModels.EnglishTests;
using CTM.Codes.CustomControls.Shared;
using CTMLib.CustomControls;
using CTMLib.CustomControls.Div;

namespace CTM.Codes.CustomControls.EnglishTests
{
    public static class FormExtension
    {
        public static MvcForm Form_Search(this AjaxHelper<Search> helper)
        {
            var htmlHelper = new HtmlHelper<Search>(helper.ViewContext, helper.ViewDataContainer);
            var model = (Search)htmlHelper.ViewDataContainer.ViewData.Model;

            var form = helper.BeginForm("Search", "EnglishTests", new {area = "Search"}, new AjaxOptions
                {
                    HttpMethod = "POST",
                    InsertionMode = InsertionMode.Replace,
                    UpdateTargetId = "search_result_table",
                    LoadingElementId = "loader",
                },
                new {id = "form_search"});

            string hidableDivId = Guid.NewGuid().ToString();
            var ccName = htmlHelper
                .TextBoxGroupFor(o => o.CCName, new { @class = "col-lg-12 col-md-12" });
            var categoryID = htmlHelper
                .DropDownListGroupFor(o => o.CategoryID, model.CateforyList,new {@class="col-lg-4 col-md-4"});
            var fromDate = htmlHelper
                .DateTimeGroupFor(o => o.FromDate, new { @class = "col-lg-4 col-md-4" });
            var toDate = htmlHelper
                .DateTimeGroupFor(o => o.ToDate, new { @class = "col-lg-4 col-md-4" });
            var isLatest = htmlHelper
                .CheckBoxGroupFor(o => o.IsLatest,new {@class="col-lg-12 col-md-12" }, new {  data_hidableDivId = hidableDivId });

            var searchBtn = htmlHelper.Button()
                .IsSubmitBtn(true)
                .SetMaterialIcon("search")
                .AddCssClass("col-lg-6 col-md-6");
            var downloadBtn = htmlHelper.Button()
                .SetMaterialIcon("file_download")
                .AddCssClass("col-lg-6 col-md-6")
                .MergeAttribute("onclick", "this.form.submit(); ");

            // Wrap
            var row1=new DivControl(ccName.ToHtmlString()).AddCssClass("row");
            var row3 = new DivControl(isLatest.ToHtmlString()).AddCssClass("row");
            var row2 = new DivControl(categoryID.ToHtmlString()
                                      + fromDate
                                      + toDate)
                .AddCssClass("row")
                .MergeAttribute("id", hidableDivId)
                .Hide();
            var row4=new DivControl(searchBtn.ToHtmlString()+downloadBtn).AddCssClass("row");

            helper.ViewContext.Writer.Write(
                row1.ToHtmlString() 
                + row2 
                + row3 
                + row4);
            return form;;
        }
    }
}