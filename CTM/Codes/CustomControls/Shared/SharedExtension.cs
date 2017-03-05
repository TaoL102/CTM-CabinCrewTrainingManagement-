using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using CTMLib.CustomControls;
using CTMLib.CustomControls.CheckBox;
using CTMLib.CustomControls.Div;
using CTMLib.CustomControls.Label;
using CTMLib.Extensions;

namespace CTM.Codes.CustomControls.Shared
{
    public static class SharedExtension
    {
        public static MvcHtmlString TextBoxGroupFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, object wrapperHtmlAttributes = null, object inputHtmlAttributes=null)
        {
            return helper.InputGroupFor(expression,wrapperHtmlAttributes,inputHtmlAttributes);
        }
        public static MvcHtmlString DateTimeGroupFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, object wrapperHtmlAttributes = null, object inputHtmlAttributes = null)
        {
            var htmlAttributes = HtmlHelperExtension.MergeAttributes(inputHtmlAttributes,
                new {@class = "datepicker", type = "text"});
            return helper.InputGroupFor(expression, wrapperHtmlAttributes, htmlAttributes);
        }
        public static MvcHtmlString PasswordGroupFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, object wrapperHtmlAttributes = null, object inputHtmlAttributes = null)
        {
            var htmlAttributes = HtmlHelperExtension.MergeAttributes(inputHtmlAttributes,
                new {  type = "password" });

            return helper.InputGroupFor(expression, wrapperHtmlAttributes, htmlAttributes);
        }
        public static MvcHtmlString FileGroupFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, object wrapperHtmlAttributes = null, object inputHtmlAttributes = null)
        {
            var htmlAttributes = HtmlHelperExtension.MergeAttributes(inputHtmlAttributes,
                new { type = "file",
                    accept = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel" ,
                    id="file",name="file"});

            return helper.InputGroupFor(expression, wrapperHtmlAttributes, htmlAttributes);
        }
        public static MvcHtmlString CheckBoxGroupFor<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, bool>> expression, object wrapperHtmlAttributes = null, object inputHtmlAttributes = null)
        {
            var expression1 = (MemberExpression)expression.Body;
            string name = expression1.Member.Name;

            var htmlAttributes = HtmlHelperExtension.AddCssClass(inputHtmlAttributes, "form-control");

            var label = helper.LabelFor(expression);
            var checkBox = new CheckBoxControl().SetId(name).SetColor(ColorOptions.Primary).SetAttributes(htmlAttributes);

            var div2=new DivControl(checkBox.ToHtmlString()).AddCssClass(" ctm-checkbox");
            var div1 = new DivControl(label+div2.ToHtmlString() ).AddCssClass("form-group").MergeAttributes(wrapperHtmlAttributes);

            return MvcHtmlString.Create(div1.ToHtmlString());
        }

        public static MvcHtmlString DropDownListGroupFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectList, object wrapperHtmlAttributes = null, object inputHtmlAttributes = null)
        {
            return GenerateDropDownListGroupFor(helper, expression, selectList, wrapperHtmlAttributes,
                inputHtmlAttributes);
        }
        public static MvcHtmlString EnumDropDownListGroupFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, object wrapperHtmlAttributes = null, object inputHtmlAttributes = null)
        {
            var selectList = expression.Body.Type.IsEnum
                ? EnumHelper.GetSelectList(expression.Body.Type)
                : null;
            return GenerateDropDownListGroupFor(helper, expression, selectList, wrapperHtmlAttributes,
    inputHtmlAttributes);
        }

        private static MvcHtmlString GenerateDropDownListGroupFor<TModel, TValue>(HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectList, object wrapperHtmlAttributes = null, object inputHtmlAttributes = null)
        {
            var htmlAttributes = HtmlHelperExtension.AddCssClass(inputHtmlAttributes, "form-control");

            var label = helper.LabelFor(expression, htmlAttributes: new { @class = "control-label" });
            var input = helper.DropDownListFor(expression, selectList, "", htmlAttributes);
            var validationMsg = helper.ValidationMessageFor(expression, "", new { @class = "text-danger" });

            var div2 = new DivControl(input + validationMsg.ToHtmlString());
            var div1 = new DivControl(label + div2.ToHtmlString()).AddCssClass("form-group").MergeAttributes(wrapperHtmlAttributes);

            return MvcHtmlString.Create(div1.ToHtmlString());
        }

        private static MvcHtmlString InputGroupFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, object wrapperHtmlAttributes = null, object inputHtmlAttributes = null)
        {
            var htmlAttributes = HtmlHelperExtension.AddCssClass(inputHtmlAttributes, "form-control");

            var label = helper.LabelFor(expression, htmlAttributes: new { @class = "control-label" });
            var input = helper.EditorFor(expression, new { htmlAttributes = htmlAttributes });
            var validationMsg = helper.ValidationMessageFor(expression, "", new { @class = "text-danger" });

            var div2 = new DivControl(input + validationMsg.ToHtmlString());
            var div1 = new DivControl(label + div2.ToHtmlString()).AddCssClass("form-group").MergeAttributes(wrapperHtmlAttributes);

            return MvcHtmlString.Create(div1.ToHtmlString());
        }
    }
}