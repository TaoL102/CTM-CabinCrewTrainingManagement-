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
        MvcForm Form_Create<TModel>(AjaxHelper<TModel> helper);
        MvcForm Form_Upload<TModel>(AjaxHelper<TModel> helper);

        TableControl Table_SearchResult<TModel>(HtmlHelper<IEnumerable<TModel>> helper, IEnumerable<TModel> models);
        TableControl Table_SearchResult_IsLatest<TModel>(HtmlHelper<IEnumerable<TModel>> helper, IEnumerable<TModel> models);

    }



    public abstract class CustomControlBase<T> : ICustomControl
    {
        protected string ControllerName { get; }

        protected CustomControlBase()
        {
            ControllerName = GetCurrentControllerName();
        }

        #region Pagination

        public PaginationControlAjax Pagination<TModel>(AjaxHelper<IEnumerable<TModel>> helper, ISearchViewModel viewModel, Pager pager)
        {
            return new PaginationControlAjax(
                helper,
                ConstantHelper.ActionNameSearch,
                ControllerName,
                ConstantHelper.AreaNameSearch,
                pager)
                .SetUpdateTargetId(ConstantHelper.FullModalContentId)
                .SetRouteValues(PaginationRouteValues(viewModel));
        }

        protected abstract object PaginationRouteValues(ISearchViewModel viewModel);

        #endregion

        #region Buttons

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
                .SetOnSuccessFun(GenerateOpenModalJSCode(ModalOptions.Message, false));

            // Set style
            obj = obj.SetMaterialIcon("delete")
                .SetColor(ColorOptions.Danger)
                .AddCssClass("btn-round")
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
                .AddCssClass("btn-round")
                .SetOnSuccessFun(GenerateOpenModalJSCode(ModalOptions.MidiumSize, true));

            // Set style
            obj = obj.SetMaterialIcon("mode_edit")
                .SetColor(ColorOptions.Info)
                .IsLinkBtn(true);

            return obj;
        }


        #endregion

        #region Forms

        public MvcForm Form_Search<TModel>(AjaxHelper<TModel> helper)
        {
            var ajaxOptions = new AjaxOptions()
            {
                InsertionMode = InsertionMode.Replace,
                UpdateTargetId = ConstantHelper.FullModalContentId,
                LoadingElementId = ConstantHelper.LoaderId,
                OnSuccess = "new function(){openModal('" + ConstantHelper.FullModalId + "',true)}"
            };
            var ajaxOptionsToHtmlAttr = ajaxOptions.ToUnobtrusiveHtmlAttributes();
            return GenerateForm(
                helper,
                ConstantHelper.ActionNameSearch,
                ConstantHelper.AreaNameSearch,
                null,
                ajaxOptionsToHtmlAttr,
                Form_Search_Body(helper));
        }
        public MvcForm Form_Create<TModel>(AjaxHelper<TModel> helper)
        {
            return GenerateForm(
                helper,
                ConstantHelper.ActionNameCreate,
                ConstantHelper.AreaNameManageData,
                null,
                null,
                Form_Create_Body(helper));
        }
        public MvcForm Form_Upload<TModel>(AjaxHelper<TModel> helper)
        {
            return GenerateForm(
                helper,
                ConstantHelper.ActionNameUpload,
                ConstantHelper.AreaNameManageData,
                null,
                new
                {
                    enctype = "multipart/form-data"
                },
                Form_Upload_Body(helper));
        }
        protected abstract string Form_Search_Body(AjaxHelper helper);
        protected abstract string Form_Create_Body(AjaxHelper helper);
        protected abstract string Form_Upload_Body(AjaxHelper helper);

        public TableControl Table_SearchResult<TModel>(HtmlHelper<IEnumerable<TModel>> helper, IEnumerable<TModel> models)
        {

            var header = Table_SearchResult_Header(helper);
            var rowsWithId = Table_SearchResult_Rows(helper, models);

            return GenerateTable(header, rowsWithId);
        }

        protected abstract string[] Table_SearchResult_Header<TModel>(HtmlHelper<IEnumerable<TModel>> helper);

        protected abstract Dictionary<string, string[]> Table_SearchResult_Rows<TModel>(HtmlHelper<IEnumerable<TModel>> helper, IEnumerable<TModel> models);

        public TableControl Table_SearchResult_IsLatest<TModel>(HtmlHelper<IEnumerable<TModel>> helper, IEnumerable<TModel> models)
        {

            var header = Table_SearchResult_IsLatest_Header(helper);
            var rowsWithId = Table_SearchResult_IsLatest_Rows(helper, models);

            return GenerateTable(header, rowsWithId);
        }

        protected abstract string[] Table_SearchResult_IsLatest_Header<TModel>(HtmlHelper<IEnumerable<TModel>> helper);

        protected abstract Dictionary<string, string[]> Table_SearchResult_IsLatest_Rows<TModel>(HtmlHelper<IEnumerable<TModel>> helper, IEnumerable<TModel> models);


        #endregion

        #region Private methods
        private TableControl GenerateTable(string[] header, Dictionary<string, string[]> rowsWithId)
        {
            return new TableControl(header, rowsWithId);
        }

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

        private MvcForm GenerateForm(AjaxHelper helper, string actionName, string areaName, object routeValues, object htmlAttributes, string formBody)
        {
            var form = helper.BeginForm(
                actionName,
                ControllerName,
                HtmlHelperExtension.AddRouteValue(routeValues, new { area = areaName }),
                new AjaxOptions
                {
                    HttpMethod = "POST"
                },
                HtmlHelperExtension.ConvertHtmlAttributesToIDictionary(htmlAttributes));

            var htmlHelper = new HtmlHelper(helper.ViewContext, helper.ViewDataContainer);

            string token = htmlHelper.AntiForgeryToken().ToHtmlString();

            helper.ViewContext.Writer.Write(token + formBody);
            return form; ;
        }

        private string GenerateOpenModalJSCode(ModalOptions modalOptions, bool isRegisterPlugins)
        {
            Dictionary<ModalOptions, string> modalsDic = new Dictionary<ModalOptions, string>()
            {
                { ModalOptions.Full,ConstantHelper.FullModalId},
                { ModalOptions.MidiumSize,ConstantHelper.MidModalId},
                { ModalOptions.Message,ConstantHelper.MsgModalId}
            };
            return "openModal('" + modalsDic[modalOptions] + "'" +
                "," + isRegisterPlugins.ToString().ToLower() + ")";
        }
        #endregion


    }
    public enum ModalOptions { Full, MidiumSize, Message }
}