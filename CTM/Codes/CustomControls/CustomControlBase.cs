using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using CTM.Areas.Search.ViewModels;
using CTM.Areas.Search.ViewModels.EnglishTests;
using CTM.Codes.Helpers;
using CTMCustomControlLib.CustomControls;
using CTMCustomControlLib.CustomControls.Button;
using CTMCustomControlLib.CustomControls.Div;
using CTMCustomControlLib.CustomControls.Pagination;
using CTMCustomControlLib.CustomControls.Table;
using CTMCustomControlLib.Extensions;
using CTMCustomControlLib.Models;
using CTMLocalizationLib.Resources;
using Microsoft.Ajax.Utilities;
using ConstantHelper = CTM.Codes.Helpers.ConstantHelper;

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
        MvcForm Form_Edit<TModel>(AjaxHelper<TModel> helper);
        MvcForm Form_Delete<TModel>(AjaxHelper<TModel> helper);

        TableControl Table_SearchResult<TModel>(HtmlHelper<IEnumerable<TModel>> helper, IEnumerable<TModel> models) where TModel : ISearchResultModel;
        TableControl Table_SearchResult_IsLatest<TModel>(HtmlHelper<IEnumerable<TModel>> helper, IEnumerable<TModel> models) where TModel : ISearchResultModel;
        Dictionary<string,string> Table_SearchResult_Row<TModel>(TModel model);
    }



    public abstract class CustomControlBase<T> : ICustomControl 
    {
        protected string ControllerName { get; }

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
            return GenerateSearchForm(
                helper,
                ConstantHelper.ActionNameSearch,
                ConstantHelper.AreaNameSearch,
                null,
                ajaxOptionsToHtmlAttr,
                Form_Search_Body(helper));
        }
        public MvcForm Form_Create<TModel>(AjaxHelper<TModel> helper)
        {
            var htmlHelper = new HtmlHelper<TModel>(helper.ViewContext, helper.ViewDataContainer);
            var button = htmlHelper.Button().SetText(ConstViews.BTN_Add).IsSubmitBtn(true);

            return GenerateForm(
                helper,
                ConstantHelper.ActionNameCreate,
                ConstantHelper.AreaNameManageData,
                null,
                null,
                Form_Create_Body(helper)+button);
        }
        public MvcForm Form_Upload<TModel>(AjaxHelper<TModel> helper)
        {
            var htmlHelper = new HtmlHelper<TModel>(helper.ViewContext, helper.ViewDataContainer);
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext, helper.RouteCollection);

            var button = htmlHelper.Button()
                .SetText(ConstViews.BTN_Upload)
                .IsSubmitBtn(true)
                .MergeAttribute("onclick", "uploadBtnClickEvent()")
                .MergeAttribute(
                    "data-uploadurl",
                    urlHelper.Action(ConstantHelper.ActionNameUpload, ControllerName,
                        new {area = ConstantHelper.AreaNameManageData}));

            return GenerateForm(
                helper,
                ConstantHelper.ActionNameUpload,
                ConstantHelper.AreaNameManageData,
                null,
                new
                {
                    enctype = "multipart/form-data"
                },
                Form_Upload_Body(helper)+button);
        }
        public MvcForm Form_Edit<TModel>(AjaxHelper<TModel> helper)
        {
            var htmlHelper = new HtmlHelper<TModel>(helper.ViewContext, helper.ViewDataContainer);
            var model = helper.ViewData.Model;
            var modelIdValue = ModelHelper<TModel>.GetPrimaryKeyValues(model)[0];
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext, helper.RouteCollection);
            var url = urlHelper.Action("GetEntityJson");
            var button = htmlHelper.Button()
                .SetText(ConstViews.BTN_Save)
                .IsSubmitBtn(true);
                
            AjaxOptions ajaxOptions=new AjaxOptions()
            {
                OnSuccess = "new function(){"+ GenerateEditElementJSCode(modelIdValue, url) + "}"

            };
        var ajaxOptionsToHtmlAttr = ajaxOptions.ToUnobtrusiveHtmlAttributes();

            return GenerateForm(
                helper,
                ConstantHelper.ActionNameEdit,
                ConstantHelper.AreaNameManageData,
                null,
                ajaxOptionsToHtmlAttr,
                Form_Edit_Body(helper)+button);
        }
        public MvcForm Form_Delete<TModel>(AjaxHelper<TModel> helper)
        {
            return GenerateForm(
                helper,
                ConstantHelper.ActionNameDelete,
                ConstantHelper.AreaNameManageData,
                null,
                null,
                Form_Delete_Body(helper));
        }
        protected abstract string Form_Search_Body(AjaxHelper helper);
        protected abstract string Form_Create_Body(AjaxHelper helper);
        protected abstract string Form_Upload_Body(AjaxHelper helper);
        protected abstract string Form_Edit_Body(AjaxHelper helper);

        protected string Form_Delete_Body<TModel>(AjaxHelper<TModel> helper)
        {
            var htmlHelper = new HtmlHelper<TModel>(helper.ViewContext, helper.ViewDataContainer);
            var model = helper.ViewData.Model;
            var modelIdName = ModelHelper<TModel>.GetPrimaryKeys()[0].Name;
            var modelIdValue = ModelHelper<TModel>.GetPrimaryKeyValues(model)[0];

            var row1 = htmlHelper.Hidden(modelIdName, modelIdValue);
            var row2= new DivControl(model.ToString());
            var row3 = htmlHelper.Button()
                .SetText(ConstViews.INFO_Delete)
                .IsSubmitBtn(true)
                .MergeAttribute("onclick", "hideElement('" + modelIdValue + "')");

            return string.Concat(row1, row2,row3);
        }
       
        #endregion

        #region Tables

        public Dictionary<string, string> Table_SearchResult_Row<TModel>(TModel model)
        {
            return ModelHelper.GetDisplayPropertyNameAndValues(model);
        }

        protected List<string> Table_SearchResult_Header<TModel>(HtmlHelper<IEnumerable<TModel>> helper)
        {
            return ModelHelper<TModel>.GetPropertyDisplayNames();
        }

        protected List<string> Table_SearchResult_IsLatest_Header<TModel>(HtmlHelper<IEnumerable<TModel>> helper)
        {
            return Table_SearchResult_Header<TModel>(helper);
        }

        protected Dictionary<string, Dictionary<string, string>> Table_SearchResult_Rows<TModel>(HtmlHelper<IEnumerable<TModel>> helper, IEnumerable<TModel> models) where TModel:ISearchResultModel
        {
            var ajaxHelper = new AjaxHelper<IEnumerable<SearchResult>>(
                helper.ViewContext, helper.ViewDataContainer);
            var modelsList = (IList<TModel>)models.ToList();
            var rows = new Dictionary<string, Dictionary<string, string>>();

            modelsList.ForEach(o =>
            {
                var dic = Table_SearchResult_Row(o);
                dic.Add(Guid.NewGuid().ToString(), ajaxHelper.Button_Edit(o.ID).ToHtmlString());
                dic.Add(Guid.NewGuid().ToString(), ajaxHelper.Button_Delete(o.ID).ToHtmlString());
                rows.Add(o.ID, dic);
            });


            return rows;

        }

        protected Dictionary<string, string[]> Table_SearchResult_IsLatest_Rows<TModel>(HtmlHelper<IEnumerable<TModel>> helper, IEnumerable<TModel> models)
        {
            var modelsList = (IList<TModel>)models.ToList();
            var rows = new Dictionary<string, string[]>();
            for (int i = 0; i < modelsList.Count(); i++)
            {
                var item = modelsList[i];
                rows.Add(
                    i.ToString(),
                    ModelHelper<TModel>.GetDisplayPropertyValues(item).ToArray());
            }

            return rows;
        }

        public TableControl Table_SearchResult<TModel>(HtmlHelper<IEnumerable<TModel>> helper, IEnumerable<TModel> models) where TModel : ISearchResultModel
        {

            var header = Table_SearchResult_Header(helper);
            var rowsWithId = Table_SearchResult_Rows(helper, models);

            return GenerateTable(header, rowsWithId);
        }

        public TableControl Table_SearchResult_IsLatest<TModel>(HtmlHelper<IEnumerable<TModel>> helper, IEnumerable<TModel> models) where TModel : ISearchResultModel
        {

            var header = Table_SearchResult_IsLatest_Header(helper);
            var rowsWithId = Table_SearchResult_IsLatest_Rows(helper, models);

            return GenerateTable(header, rowsWithId);
        }



        #endregion

        #region methods

        protected CustomControlBase()
        {
            ControllerName = GetCurrentControllerName();
        }

        private TableControl GenerateTable(List<string> header, Dictionary<string, string[]> rowsWithId)
        {
            return new TableControl(header.ToArray(), rowsWithId);
        }

        private TableControl GenerateTable(List<string> header, Dictionary<string, Dictionary<string, string>> rowsWithIdAndTrWithNameAttr)
        {
            return new TableControl(header.ToArray(), rowsWithIdAndTrWithNameAttr);
        }

        private string GetCurrentControllerName()
        {
            return ControllerHelper<T>.GetControllerName();
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
            return form; 
        }

        private MvcForm GenerateSearchForm(AjaxHelper helper, string actionName, string areaName, object routeValues, object htmlAttributes, string formBody)
        {
            var htmlHelper = new HtmlHelper(helper.ViewContext, helper.ViewDataContainer);

            // Search & Download buttons
            var searchBtn = htmlHelper.Button()
                .IsSubmitBtn(true)
                .SetMaterialIcon("search")
                .AddCssClass("col-sm-6");
            var downloadBtn = htmlHelper.Button()
                .SetMaterialIcon("file_download")
                .AddCssClass("col-sm-6")
                .MergeAttribute("onclick", "downloadBtnClickEvent(); ");
            var buttonWrapper = new DivControl(searchBtn.ToHtmlString() + downloadBtn)
                .AddCssClass("col-12");

            var row = new DivControl(buttonWrapper.ToString()).AddCssClass("row");

            formBody += row;

            return GenerateForm(helper, actionName, areaName, routeValues, htmlAttributes, formBody);
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

        private string GenerateEditElementJSCode(string id,string url)
        {
            return "editElement('" + ControllerName + "','" + id + "','" + url + "')";
        }

        #endregion


    }
    public enum ModalOptions { Full, MidiumSize, Message }
}