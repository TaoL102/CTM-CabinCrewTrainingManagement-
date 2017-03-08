using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using CTM.Areas.ManageData.ViewModels.EnglishTests;
using CTM.Areas.Search.ViewModels;
using CTM.Areas.Search.ViewModels.EnglishTests;
using CTM.Codes.CustomControls.Shared;
using CTMLib.CustomControls.Div;
using CTMLib.Extensions;
using CTMLib.Models;
using CTMLib.Resources;
using CTMLib.Helpers;

namespace CTM.Codes.CustomControls.EnglishTests
{
    public class CustomControlEnglishTest : CustomControlBase<EnglishTest>
    {

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

        protected override Dictionary<string, Dictionary<string, string>> Table_SearchResult_Rows<TModel>(HtmlHelper<IEnumerable<TModel>> helper, IEnumerable<TModel> models)
        {
            var ajaxHelper = new AjaxHelper<IEnumerable<SearchResult>>(
    helper.ViewContext, helper.ViewDataContainer);

            var rowsWithIdAndTrWithNameAttr = new Dictionary<string, Dictionary<string, string>>();
            foreach (var item in (IEnumerable<SearchResult>)models)
            {
                rowsWithIdAndTrWithNameAttr.Add(item.ID,new Dictionary < string, string>()
                {
                    { ModelHelper<TModel>.GetPropertyName(o=>item.CabinCrewName),helper.DisplayValueFor(o=>item.CabinCrewName).ToString()},
                    { ModelHelper<TModel>.GetPropertyName(o=>item.Type),helper.DisplayValueFor(o=>item.Type).ToString()},
                    { ModelHelper<TModel>.GetPropertyName(o=>item.Grade),helper.DisplayValueFor(o=>item.Grade).ToString()},
                    { ModelHelper<TModel>.GetPropertyName(o=>item.CategoryName),helper.DisplayValueFor(o=>item.CategoryName).ToString()},
                    { ModelHelper<TModel>.GetPropertyName(o=>item.Date),helper.DisplayValueFor(o=>item.Date).ToString()},
                    { Guid.NewGuid().ToString(), ajaxHelper.Button_Edit(item.ID).ToHtmlString()},
                    {Guid.NewGuid().ToString(), ajaxHelper.Button_Delete(item.ID).ToHtmlString()},

                });
            }
            return rowsWithIdAndTrWithNameAttr; ;
        }
        public override Dictionary<string, string> Table_SearchResult_Row(object model)
        {
            var searchResult = (SearchResult) model;
            var rowsWithIdAndTrWithNameAttr = new Dictionary<string,string>
            {
                        {
                            ModelHelper<Search>.GetPropertyName(o => searchResult.CabinCrewName),
                            ModelHelper<Search>.GetPropertyDisplayValue(o => searchResult.CabinCrewName, model)
                        },
                        {
                            ModelHelper<Search>.GetPropertyName(o => searchResult.Type),
                            ModelHelper<Search>.GetPropertyDisplayValue(o => searchResult.Type, model)
                        },
                        {
                            ModelHelper<Search>.GetPropertyName(o => searchResult.Grade),
                            ModelHelper<Search>.GetPropertyDisplayValue(o => searchResult.Grade, model)
                        },
                        {
                            ModelHelper<Search>.GetPropertyName(o => searchResult.CategoryName),
                            ModelHelper<Search>.GetPropertyDisplayValue(o => searchResult.CategoryName, model)
                        },
                        {
                            ModelHelper<Search>.GetPropertyName(o => searchResult.Date),
                            ModelHelper<Search>.GetPropertyDisplayValue(o => searchResult.Date, model)
                        },
            };


            return rowsWithIdAndTrWithNameAttr; 
        }

        protected override string[] Table_SearchResult_Header<TModel>(HtmlHelper<IEnumerable<TModel>> helper)
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

            return header; ;
        }

        protected override string[] Table_SearchResult_IsLatest_Header<TModel>(HtmlHelper<IEnumerable<TModel>> helper)
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

        protected override Dictionary<string, string[]> Table_SearchResult_IsLatest_Rows<TModel>(HtmlHelper<IEnumerable<TModel>> helper, IEnumerable<TModel> models)
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