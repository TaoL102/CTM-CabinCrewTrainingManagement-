using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using CTM.Codes.Helpers;

namespace CTM.Codes.Extensions
{
    public static class HtmlHelperExtension
    {
        public static MvcHtmlString DisplayValueFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression)
        {
            var value = ModelMetadata.FromLambdaExpression(expression, html.ViewData).Model;
            if (value is Enum)
            {
                return MvcHtmlString.Create(ModelHelper.GetEnumPropertyValue(value as Enum));
            }
            return value != null ? html.DisplayFor(expression) : MvcHtmlString.Empty;
        }
    }
}