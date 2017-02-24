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

            var ccName = htmlHelper.TextBoxGroupFor(o => o.CCName);
            var categoryID = htmlHelper.DropDownListGroupFor(o => o.CategoryID, model.CateforyList);
            var fromDate = htmlHelper.DateTimeGroupFor(o => o.FromDate);
            var toDate = htmlHelper.DateTimeGroupFor(o => o.ToDate);
            var isLatest = htmlHelper.CheckBoxGroupFor(o => o.IsLatest);

            var searchBtn = htmlHelper.Button().IsSubmitBtn(true).SetMaterialIcon("search");
            var downloadBtn = htmlHelper.Button()
                .SetMaterialIcon("file_download")
                .MergeAttribute("onclick", "this.form.submit(); ");

            helper.ViewContext.Writer.Write(
                ccName.ToHtmlString() 
                + categoryID 
                + fromDate 
                + toDate+isLatest
                +searchBtn
                + downloadBtn);
            return form;;
        }
    }
}