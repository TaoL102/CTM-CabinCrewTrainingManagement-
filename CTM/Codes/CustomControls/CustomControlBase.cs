using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using CTM.Areas.Search.ViewModels;
using CTM.Areas.Search.ViewModels.EnglishTests;
using CTM.Codes.CustomControls.Shared;
using CTM.Codes.Helpers;
using CTMLib.CustomControls;
using CTMLib.CustomControls.Button;
using CTMLib.CustomControls.Div;
using CTMLib.CustomControls.Pagination;
using CTMLib.CustomControls.Table;
using CTMLib.Extensions;
using CTMLib.Models;

namespace CTM.Codes.CustomControls
{
    public interface ICustomControl
    {
        ButtonControlAjax Button_Delete<TModel>(AjaxHelper<IEnumerable<TModel>> helper, string rowId);
        ButtonControlAjax Button_Edit<TModel>(AjaxHelper<IEnumerable<TModel>> helper, string rowId);
        PaginationControlAjax Pagination<TModel>(AjaxHelper<IEnumerable<TModel>> helper, ISearchViewModel viewModel, Pager pager);
        MvcForm Form_Search<TModel>(AjaxHelper<TModel> helper);
        TableControl Table_SearchResult<TModel>(HtmlHelper<IEnumerable<TModel>> helper, IEnumerable<TModel> models);
        TableControl Table_SearchResult_IsLatest<TModel>(HtmlHelper<IEnumerable<TModel>> helper, IEnumerable<TModel> models);

    }



    public abstract class CustomControlBase<T> : ICustomControl
    {
        protected string ControllerName { get;}

        protected CustomControlBase()
        {
            ControllerName = GetCurrentControllerName();
        }

        public PaginationControlAjax Pagination<TModel>(AjaxHelper<IEnumerable<TModel>> helper,ISearchViewModel viewModel, Pager pager)
        {
            return new PaginationControlAjax(
                helper,
                ConstantHelper.ActionNameSearch,
                ControllerName,
                ConstantHelper.AreaNameSearch,
                pager)
                .SetUpdateTargetId(ConstantHelper.FullModalContentId)
                .SetRouteValues(GetPaginationRouteValues(viewModel));
        }

        protected abstract object GetPaginationRouteValues(ISearchViewModel viewModel);

        public ButtonControlAjax Button_Delete<TModel>(AjaxHelper<IEnumerable<TModel>> helper, string rowId)
        {
            // Data
            var routeValues = new { id = rowId };

            // Set data
            var obj = helper.Button(
                    ConstantHelper.ActionNameDelete,
                    ControllerName,
                    ConstantHelper.AreaNameManageData)
                .SetUpdateTargetId(ConstantHelper.MsgModalContentId)
                .SetRouteValues(routeValues)
                .SetOnSuccessFun("openModal('" + ConstantHelper.MsgModalId + "')");

            // Set style
            obj = obj.SetMaterialIcon("delete")
                .SetColor(ColorOptions.Danger)
                .IsLinkBtn(true);

            return obj;
        }

        public ButtonControlAjax Button_Edit<TModel>(AjaxHelper<IEnumerable<TModel>> helper, string rowId)
        {
            // Data
            var routeValues = new { id = rowId };

            // Set data
            var obj = helper.Button(
                    ConstantHelper.ActionNameEdit,
                    ControllerName,
                    ConstantHelper.AreaNameManageData)
                .SetUpdateTargetId(ConstantHelper.MidModalContentId)
                .SetRouteValues(routeValues)
                .SetOnSuccessFun("openModal('" + ConstantHelper.MidModalId + "',true)");

            // Set style
            obj = obj.SetMaterialIcon("mode_edit")
                .SetColor(ColorOptions.Info)
                .IsLinkBtn(true);

            return obj;
        }

        public MvcForm Form_Search<TModel>(AjaxHelper<TModel> helper)
        {
            var form = helper.BeginForm(ConstantHelper.ActionNameSearch, ControllerName, new { area = "Search" }, new AjaxOptions
            {
                HttpMethod = "POST",
                InsertionMode = InsertionMode.Replace,
                UpdateTargetId = ConstantHelper.FullModalContentId,
                LoadingElementId = ConstantHelper.LoaderId,
                OnSuccess = "new function(){openModal('" + ConstantHelper.FullModalId + "',true)}"
            },
                null);

            string formBody = GenerateSearchFormBody(helper);

            helper.ViewContext.Writer.Write(
                formBody);
            return form; ;
        }

        protected abstract string GenerateSearchFormBody(AjaxHelper helper);

        public TableControl Table_SearchResult<TModel>(HtmlHelper<IEnumerable<TModel>> helper, IEnumerable<TModel> models)
        {

            var header = GetSearchResultHeader(helper);
            var rowsWithId = GetSearchResultRowsWithId(helper,models);

            return GenerateTable(header, rowsWithId);
        }

        protected abstract string[] GetSearchResultHeader<TModel>(HtmlHelper<IEnumerable<TModel>> helper);

        protected abstract Dictionary<string, string[]> GetSearchResultRowsWithId<TModel>(HtmlHelper<IEnumerable<TModel>> helper, IEnumerable<TModel> models);

        protected TableControl GenerateTable(string[] header, Dictionary<string, string[]> rowsWithId)
        {
            return new TableControl(header, rowsWithId);
        }
        public TableControl Table_SearchResult_IsLatest<TModel>(HtmlHelper<IEnumerable<TModel>> helper, IEnumerable<TModel> models)
        {

            var header = GetSearchResultHeader_IsLatest(helper);
            var rowsWithId = GetSearchResultRowsWithId_IsLatest(helper, models);

            return GenerateTable(header, rowsWithId);
        }

        protected abstract string[] GetSearchResultHeader_IsLatest<TModel>(HtmlHelper<IEnumerable<TModel>> helper);

        protected abstract Dictionary<string, string[]> GetSearchResultRowsWithId_IsLatest<TModel>(HtmlHelper<IEnumerable<TModel>> helper, IEnumerable<TModel> models);

        private string GetCurrentControllerName()
        {
            var nameSpace =
                typeof(T).Namespace;
            if (nameSpace.Contains(ConstantHelper.ControllerNameEnglishTest))
            {
                return ConstantHelper.ControllerNameEnglishTest;
            }
            else if (nameSpace.Contains(ConstantHelper.ControllerNameRefresherTraining))
            {
                return ConstantHelper.ControllerNameRefresherTraining;
            }
            else if (nameSpace.Contains(ConstantHelper.ControllerNameCabinCrew))
            {
                return ConstantHelper.ControllerNameCabinCrew;
            }
            else if (nameSpace.Contains(ConstantHelper.ControllerNameCategory))
            {
                return ConstantHelper.ControllerNameCategory;
            }
            else if (nameSpace.Contains(ConstantHelper.ControllerNameLog))
            {
                return ConstantHelper.ControllerNameLog;
            }
            else if (nameSpace.Contains(ConstantHelper.ControllerNameUploadRecord))
            {
                return ConstantHelper.ControllerNameUploadRecord;
            }
            return null;
        }


    }
}