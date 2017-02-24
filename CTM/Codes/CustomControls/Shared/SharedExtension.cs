using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using CTMLib.CustomControls.Div;
using CTMLib.Extensions;

namespace CTM.Codes.CustomControls.Shared
{
    public static class SharedExtension
    {
        public static MvcHtmlString TextBoxGroupFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression)
        {
            var label = helper.LabelFor(expression, htmlAttributes: new { @class = "control-label col-md-2" });
            var input = helper.EditorFor(expression, new { htmlAttributes = new { @class = "form-control" } });
            var validationMsg = helper.ValidationMessageFor(expression, "", new { @class = "text-danger" });

            var div2 = new DivControl(input + validationMsg.ToHtmlString()).AddCssClass("col-md-10");
            var div1 = new DivControl(label + div2.ToHtmlString()).AddCssClass("form-group");

            return MvcHtmlString.Create(div1.ToHtmlString());
        }
        public static MvcHtmlString DateTimeGroupFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression)
        {
            var label = helper.LabelFor(expression, htmlAttributes: new { @class = "control-label col-md-2" });
            var input = helper.EditorFor(expression, new { htmlAttributes = new { @class = "form-control",type="date" } });
            var validationMsg = helper.ValidationMessageFor(expression, "", new { @class = "text-danger" });

            var div2 = new DivControl(input + validationMsg.ToHtmlString()).AddCssClass("col-md-10");
            var div1 = new DivControl(label + div2.ToHtmlString()).AddCssClass("form-group");

            return MvcHtmlString.Create(div1.ToHtmlString());
        }
        public static MvcHtmlString CheckBoxGroupFor<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, bool>> expression)
        {
            var label = helper.LabelFor(expression);
            var input = helper.CheckBoxFor(expression, new { htmlAttributes = new { @class = "form-control" } });
            
            var div1 = new DivControl(input+label.ToHtmlString()).AddCssClass("form-group");

            return MvcHtmlString.Create(div1.ToHtmlString());
        }

        public static MvcHtmlString DropDownListGroupFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression,SelectList selectList)
        {
            var label = helper.LabelFor(expression, htmlAttributes: new { @class = "control-label col-md-2" });
            var input = helper.DropDownListFor(expression,selectList,"", new {  @class = "form-control"  });
            var validationMsg = helper.ValidationMessageFor(expression, "", new { @class = "text-danger" });

            var div2 = new DivControl(input + validationMsg.ToHtmlString()).AddCssClass("col-md-10");
            var div1 = new DivControl(label + div2.ToHtmlString()).AddCssClass("form-group");

            return MvcHtmlString.Create(div1.ToHtmlString());
        }
    }
}