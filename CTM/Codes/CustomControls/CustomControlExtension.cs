using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using CTM.Areas.Search.ViewModels;
using CTM.Areas.Search.ViewModels.EnglishTests;
using CTM.Codes.CustomControls.EnglishTests;
using CTM.Codes.Helpers;
using CTMLib.CustomControls.Button;
using CTMLib.CustomControls.Pagination;
using CTMLib.CustomControls.Table;
using CTMLib.Extensions;
using CTMLib.Models;

namespace CTM.Codes.CustomControls
{
    public static class CustomControlExtension
    {
        public static ButtonControlAjax Button_Delete<T>(this AjaxHelper<IEnumerable<T>> helper, string rowId)
        {
            return GetCustomControl<T>().Button_Delete(helper,rowId);
        }

        public static ButtonControlAjax Button_Edit<T>(this AjaxHelper<IEnumerable<T>> helper, string rowId)
        {
            return GetCustomControl<T>().Button_Edit(helper, rowId);
        }

        public static MvcForm Form_Search<T>(this AjaxHelper<T> helper)
        {
            return GetCustomControl<T>().Form_Search(helper);
        }
        public static MvcForm Form_Create<T>(this AjaxHelper<T> helper)
        {
            return GetCustomControl<T>().Form_Create(helper);
        }
        public static MvcForm Form_Upload<T>(this AjaxHelper<T> helper)
        {
            return GetCustomControl<T>().Form_Upload(helper);
        }

        public static PaginationControlAjax Pagination<T>(this AjaxHelper<IEnumerable<T>> helper,
    ISearchViewModel searchViewModel, Pager pager)
        {
            return GetCustomControl<T>().Pagination(helper, searchViewModel, pager);
        }

        public static TableControl Table_SearchResult<T>(this HtmlHelper<IEnumerable<T>> helper,
            IEnumerable<T> models)
        {
            return GetCustomControl<T>().Table_SearchResult(helper, models);
        }
        public static TableControl Table_SearchResult_IsLatest<T>(this HtmlHelper<IEnumerable<T>> helper,
    IEnumerable<T> models)
        {
            return GetCustomControl<T>().Table_SearchResult_IsLatest(helper, models);
        }


        private static ICustomControl GetCustomControl<T>()
        {
            var nameSpace =
                typeof(T).Namespace;
            if (nameSpace.Contains(ConstantHelper.ControllerNameEnglishTest))
            {
                return new CustomControlEnglishTest();
            }
            //else if (nameSpace.Contains(ConstantHelper.ControllerNameRefresherTraining))
            //{
            //    return ConstantHelper.ControllerNameRefresherTraining;
            //}
            //else if (nameSpace.Contains(ConstantHelper.ControllerNameCabinCrew))
            //{
            //    return ConstantHelper.ControllerNameCabinCrew;
            //}
            //else if (nameSpace.Contains(ConstantHelper.ControllerNameCategory))
            //{
            //    return ConstantHelper.ControllerNameCategory;
            //}
            //else if (nameSpace.Contains(ConstantHelper.ControllerNameLog))
            //{
            //    return ConstantHelper.ControllerNameLog;
            //}
            //else if (nameSpace.Contains(ConstantHelper.ControllerNameUploadRecord))
            //{
            //    return ConstantHelper.ControllerNameUploadRecord;
            //}
            return null;
        }
    }
}