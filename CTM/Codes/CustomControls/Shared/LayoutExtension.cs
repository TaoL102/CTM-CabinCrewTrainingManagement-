using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CTMLib.CustomControls;
using CTMLib.CustomControls.Div;
using CTMLib.Extensions;
using System.Web.Mvc;
using CTMLib.CustomControls.Modal;

namespace CTM.Codes.CustomControls.Shared
{
    public static class LayoutExtension
    {
        public static ModalControl DisplayModal(this HtmlHelper helper)
        {
            var div=new DivControl().SetId("mid_size_modal_content");
            var obj = helper.Modal("mid_size_modal", "Confirm")
                .SetBodyHtml("mid_size_modal_content", div.ToHtmlString());
            return obj;
        }
        public static ModalControl MsgModal(this HtmlHelper helper)
        {
            var div = new DivControl().SetId("msg_modal_content");
            var obj = helper.Modal("msg_modal", "Confirm")
                .SetSize(SizeOptions.Small)
                .SetBodyHtml("msg_modal_content", div.ToHtmlString());
            return obj;
        }
    }
}