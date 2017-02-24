using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.UI;
using CTM.Areas.Search.ViewModels.EnglishTests;
using CTM.Codes.Helpers;
using CTMLib.CustomControls;
using CTMLib.CustomControls.Button;
using CTMLib.Extensions;

namespace CTM.Codes.CustomControls.EnglishTests
{

    public static class ButtonExtension
    {
        private static readonly string ActionNameDelete = ConstantHelper.ActionNameDelete;
        private static readonly string ActionNameEdit = ConstantHelper.ActionNameEdit;
        private static readonly string ActionNameCreate = ConstantHelper.ActionNameCreate;
        private static readonly string ControllerName = ConstantHelper.ControllerNameEnglishTest;
        private static readonly string AreaNameAdminData = ConstantHelper.AreaNameAdminData;
        private static readonly string AreaNameSearch = ConstantHelper.AreaNameSearch;

        public static ButtonControl Button_Delete(this HtmlHelper<IEnumerable<SearchResult>> helper, string rowId)
        {
            // Data
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            var dataUrl = urlHelper.Action(ActionNameDelete, ControllerName, new { area = AreaNameAdminData, id = rowId });
            var htmlAttributesDic = new Dictionary<string, object>()
            {
                {"href", "#message_box_modal"},
                {"data-toggle", "modal"},
                {"data-rowid", rowId},
                {"data-url", dataUrl},
                {"class", " btn_del "}
            };

            // Set data
            var obj = helper.Button()
                .SetAttributes(htmlAttributesDic);

            // Set style
            obj = obj.SetMaterialIcon("delete")
                .IsLinkBtn(true)
                .SetColor(ColorOptions.Danger);

            return obj;
        }

        public static ButtonControlAjax Button_Edit(this AjaxHelper<IEnumerable<SearchResult>> helper, string rowId)
        {
            // Data
            var routeValues = new { area = "ManageData", id = rowId };

            // Set data
            var obj = helper.Button(ActionNameEdit, ControllerName, "mid_size_modal_content")
                .SetRouteValues(routeValues)
                .SetOnSuccessFun("openMidSizeModal");

            // Set style
            obj = obj.SetMaterialIcon("mode_edit")
                .SetColor(ColorOptions.Primary).
                IsLinkBtn(true);

            return obj;
        }
    }
}