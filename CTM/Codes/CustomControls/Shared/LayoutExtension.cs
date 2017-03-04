using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CTMLib.CustomControls;
using CTMLib.CustomControls.Div;
using CTMLib.Extensions;
using System.Web.Mvc;
using CTM.Codes.Helpers;
using CTMLib.CustomControls.Modal;

namespace CTM.Codes.CustomControls.Shared
{
    public static class LayoutExtension
    {
        public static ModalControl MidSizeModal(this HtmlHelper helper)
        {
            var div=new DivControl().SetId(ConstantHelper.MidModalContentId);
            var obj = helper.Modal(ConstantHelper.MidModalId, "Confirm")
                .SetBodyHtml(ConstantHelper.MidModalContentId, div.ToHtmlString());
            return obj;
        }
        public static ModalControl MsgModal(this HtmlHelper helper)
        {
            var div = new DivControl().SetId(ConstantHelper.MsgModalContentId);
            var obj = helper.Modal(ConstantHelper.MsgModalId, "Confirm")
                .SetSize(SizeOptions.Small)
                .SetBodyHtml(ConstantHelper.MsgModalContentId, div.ToHtmlString());
            return obj;
        }
        public static ModalControl FullSizeModal(this HtmlHelper helper)
        {
            // Body
            var div = new DivControl().SetId(ConstantHelper.FullModalContentId);
            var obj = helper.Modal(ConstantHelper.FullModalId, "Confirm")
                .SetSize(SizeOptions.Full)
                .AddCssClass("ctm-modal-full")
                .SetBodyHtml(ConstantHelper.FullModalContentId, div.ToHtmlString());
            return obj;
        }
    }
}