using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using CTMLib.CustomControls;
using CTMLib.CustomControls.Alert;
using CTMLib.CustomControls.Button;
using CTMLib.CustomControls.Modal;
using CTMLib.CustomControls.Pagination;
using CTMLib.Helpers;
using CTMLib.Models;
using WebGrease.Css.Extensions;

namespace CTMLib.Extensions
{
    /// <summary>
    /// Custom Html Helpers with extensions 
    /// Reference: https://www.asp.net/mvc/overview/older-versions-1/views/creating-custom-html-helpers-cs
    /// </summary>
    public static class HtmlHelperExtension
    {
        public static Dictionary<string, object> ConvertHtmlAttributesToIDictionary(object htmlAttributes)
        {
            if (htmlAttributes == null)
            {
                return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            }
            var objects = htmlAttributes as Dictionary<string, object>;
            Dictionary<string, object> htmlAttributesDic =
                objects != null
                ? new Dictionary<string, object>(objects, StringComparer.OrdinalIgnoreCase)
                : new Dictionary<string, object>(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), StringComparer.OrdinalIgnoreCase);
            return htmlAttributesDic;
        }

        public static RouteValueDictionary ConvertRouteValuesToIDictionary(object routeValues)
        {
            var dictionary = routeValues as RouteValueDictionary;
            if (dictionary != null)
            {
                return dictionary;
            }
            return new RouteValueDictionary(routeValues); ;

        }



        public static Dictionary<string, object> AddCssClass(object htmlAttributes, string cssClass)
        {
            return AddAttToDic(htmlAttributes, "class", cssClass);
        }

        public static Dictionary<string, object> RemoveCssClass(object htmlAttributes, string cssClass)
        {
            return RemoveAttInDic(htmlAttributes, "class", cssClass);
        }

        public static Dictionary<string, object> RemoveAllCssClass(object htmlAttributes)
        {
            return RemoveAttInDic(htmlAttributes, "class");
        }

        public static RouteValueDictionary AddRouteValue(object originalRouteValues, object addedRouteValues)
        {
            var originalRouteValuesDic = ConvertRouteValuesToIDictionary(originalRouteValues);
            var addedRouteValuesDic = ConvertRouteValuesToIDictionary(addedRouteValues);

            var dic = Enumerable.Concat<KeyValuePair<string, object>>(originalRouteValuesDic, addedRouteValuesDic).GroupBy(o => o.Key).ToDictionary(o => o.Key, v => v.First().Value);

            var rdic = new RouteValueDictionary();
            foreach (var VARIABLE in dic)
            {
                rdic.Add(VARIABLE.Key, VARIABLE.Value);
            }

            return rdic;
        }

        public static Dictionary<string, object> MergeAttribute(object htmlAttributes, string key, string value)
        {
            return AddAttToDic(htmlAttributes, key, value);
        }

        public static Dictionary<string, object> MergeAttributes(object oriHtmlAttributes, object curHtmlAttributes)
        {
            var curDic = ConvertHtmlAttributesToIDictionary(curHtmlAttributes);
            var oriDic = ConvertHtmlAttributesToIDictionary(oriHtmlAttributes);
            curDic.ForEach(o =>
            {
                oriDic = AddAttToDic(oriDic, o.Key, o.Value.ToString());
            });
            return oriDic;
        }

        private static Dictionary<string, object> AddAttToDic(object htmlAttributes, string key, string value)
        {
            var htmlAttributesDic = ConvertHtmlAttributesToIDictionary(htmlAttributes);
            if (htmlAttributesDic.ContainsKey(key))
            {
                htmlAttributesDic[key] += " " + value + " ";
            }
            else
            {
                htmlAttributesDic.Add(key, value);
            }
            return htmlAttributesDic;
        }

        private static Dictionary<string, object> RemoveAttInDic(object htmlAttributes, string key, string value = null)
        {
            var htmlAttributesDic = ConvertHtmlAttributesToIDictionary(htmlAttributes);
            if (htmlAttributesDic.ContainsKey(key))
            {
                if (value == null)
                {
                    htmlAttributesDic[key] = "";
                }
                else
                {
                    htmlAttributesDic[key] = htmlAttributesDic[key].ToString().Replace(value, "");
                }
            }
            return htmlAttributesDic;
        }

        public static bool IsActiveLink(this HtmlHelper helper, string area, string controller, string action)
        {
            var curController = helper.ViewContext.RouteData.Values["controller"].ToString();
            var curAction = helper.ViewContext.RouteData.Values["action"].ToString();
            var curArea = helper.ViewContext.RouteData.DataTokens["area"].ToString();

            var isActiveLink = curArea == area && curController == controller && curAction == action;

            return isActiveLink;
        }

        public static MvcHtmlString DisplayValueFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression)
        {
            var value = ModelMetadata.FromLambdaExpression(expression, html.ViewData).Model;
            if (value is Enum)
            {
                return MvcHtmlString.Create(ModelHelper<TModel>.GetEnumPropertyValue(value as Enum));
            }
            return value != null ? html.DisplayFor(expression) : MvcHtmlString.Empty;
        }



        public static MvcHtmlString Table_Row(this HtmlHelper helper, string id, object trHtmlAttributes, Dictionary<int, object> tdContentDic, Dictionary<int, object> tdHtmlAttributesDic)
        {
            // Create tag builder
            var builder = new TagBuilder("tr");

            // Create valid id
            if (!string.IsNullOrEmpty(id))
            {
                builder.GenerateId(id);
            }

            // Add attributes
            builder.MergeAttributes(ConvertHtmlAttributesToIDictionary(trHtmlAttributes));

            // Generate tr
            MvcHtmlString tdHtmlString = Table_Row_No_Tr(helper, tdContentDic, tdHtmlAttributesDic);

            // add td to tr
            builder.InnerHtml += MvcHtmlString.Create(tdHtmlString.ToString());
            // Render tag
            return MvcHtmlString.Create(builder.ToString());
        }

        public static MvcHtmlString Table_Row_No_Tr(this HtmlHelper helper, Dictionary<int, object> tdContentDic, Dictionary<int, object> tdHtmlAttributesDic)
        {
            String htmlString = "";

            // Generate tr
            for (int i = 1; i <= tdContentDic.Count; i++)
            {
                var colBuilder = new TagBuilder("td");

                // Put content in td
                colBuilder.InnerHtml = tdContentDic[i].ToString();

                // If td has html attributes, merge attributes
                if (tdHtmlAttributesDic != null)
                {
                    if (tdHtmlAttributesDic.ContainsKey(i))
                    {
                        colBuilder.MergeAttributes(ConvertHtmlAttributesToIDictionary(tdHtmlAttributesDic[i]));
                    }
                }


                htmlString += MvcHtmlString.Create(colBuilder.ToString());

            }

            // Render tag
            return MvcHtmlString.Create(htmlString);
        }


        public static ButtonControl Button(this HtmlHelper helper)
        {
            return new ButtonControl();
        }


        public static ButtonControlAjax Button(this AjaxHelper helper, string actionName, string controllerName, string areaName)
        {
            return new ButtonControlAjax(helper, actionName, controllerName, areaName);
        }
        public static AlertControl Alert(this HtmlHelper html, string text)
        {
            return new AlertControl(text);
        }

        public static AlertControl AlertFor<TModel, TTextProperty, TStyleProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TTextProperty>> textExpression)
        {
            var text = (string)ModelMetadata.FromLambdaExpression(textExpression, html.ViewData).Model;

            return new AlertControl(text);
        }

        public static ModalControl Modal(this HtmlHelper helper, string id, string title)
        {
            return new ModalControl(id, title);
        }
    }

}