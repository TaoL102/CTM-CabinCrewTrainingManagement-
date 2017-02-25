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
using CTMLib.Models;

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

        public static ButtonControlAjax Button_Delete(this AjaxHelper<IEnumerable<SearchResult>> helper, string rowId)
        {
            // Data
            var routeValues = new { area = "ManageData", id = rowId };

            // Set data
            var obj = helper.Button(ActionNameDelete, ControllerName, "msg_modal_content")
                .SetRouteValues(routeValues)
                .SetOnSuccessFun("openMsgModal");

            // Set style
            obj = obj.SetMaterialIcon("delete")
                .SetColor(ColorOptions.Danger)
                .IsLinkBtn(true);

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
                .SetColor(ColorOptions.Danger)
                .IsLinkBtn(true);

            return obj;
        }
    }
}