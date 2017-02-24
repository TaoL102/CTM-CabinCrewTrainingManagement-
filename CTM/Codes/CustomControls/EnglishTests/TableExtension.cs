using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CTM.Areas.Search.ViewModels.EnglishTests;
using CTMLib.CustomControls.Table;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using CTMLib.Extensions;
using CTMLib.Helpers;
using WebGrease.Css.Extensions;

namespace CTM.Codes.CustomControls.EnglishTests
{
    public static class TableExtension
    {
        public static TableControl Table_SearchResult(this HtmlHelper<IEnumerable<SearchResult>> helper, IEnumerable<SearchResult> models)
        {
            var ajaxHelper = new AjaxHelper<IEnumerable<SearchResult>>(
                helper.ViewContext, helper.ViewDataContainer);

            // Data
            var header = new string[]
            {
                helper.DisplayNameFor(o => o.CabinCrewName).ToString(),
                helper.DisplayNameFor(o => o.Type).ToString(),
                helper.DisplayNameFor(o => o.Grade).ToString(),
                helper.DisplayNameFor(o => o.CategoryName).ToString(),
                helper.DisplayNameFor(o => o.Date).ToString(),
                "",""
            };

            var rowsWithId = new Dictionary<string, string[]>();
            foreach (var item in models)
            {
                rowsWithId.Add(item.ID, new string[]
                {
                    helper.DisplayValueFor(o=>item.CabinCrewName).ToString(),
                    helper.DisplayValueFor(o=>item.Type).ToString(),
                    helper.DisplayValueFor(o=>item.Grade).ToString() ,
                    helper.DisplayValueFor(o=>item.CategoryName).ToString(),
                    helper.DisplayValueFor(o=>item.Date).ToString(),
                    ajaxHelper.Button_Edit(item.ID).ToHtmlString(),
                    helper.Button_Delete(item.ID).ToHtmlString(),
                });
            }

            return new TableControl(header, rowsWithId);
        }

        public static TableControl Table_SearchResult_IsLatest(this HtmlHelper<IEnumerable<SearchResultIsLatest>> helper,
            IEnumerable<SearchResultIsLatest> models)
        {
            // Data
            var header = new string[]
            {
                helper.DisplayNameFor(o => o.CabinCrewName).ToString(),
                LocalizationHelper.GetModelString("CabinAnnoucement"),
                "","",
                LocalizationHelper.GetModelString("SpokenSkill"),
                 "","",
            };

            var modesList = models as IList<SearchResultIsLatest> ?? models.ToList();
            var rows = new Dictionary<string, string[]>();
            for (int i = 0; i < modesList.Count(); i++)
            {
                var item = modesList[i];
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

            return new TableControl(header, rows);
        }
    }
}