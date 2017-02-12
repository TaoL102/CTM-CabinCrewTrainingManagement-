using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using CTM.Codes.CustomControls;

namespace CTM.Codes.Extensions
{
    /// <summary>
    /// Custom Html Helpers with extensions 
    /// Reference: https://www.asp.net/mvc/overview/older-versions-1/views/creating-custom-html-helpers-cs
    /// </summary>
    public static class HtmlHelperExtension
    {
        public static Dictionary<string, object> ConvertHtmlAttributesToIDictionary(object htmlAttributes)
        {
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
            return  new RouteValueDictionary(routeValues); ;
        }



        private static string GetEnumPropertyValue(Enum enumValue)
        {
            try
            {
                return Resources.Models.ConstModels.ResourceManager.GetString(enumValue.ToString());
            }
             catch (Exception e)
            {
                return enumValue.ToString();
            }
                
        }

        public static Dictionary<string, object> AddCssClass(object htmlAttributes, string cssClass)
        {
            var htmlAttributesDic = ConvertHtmlAttributesToIDictionary(htmlAttributes);
            if (htmlAttributesDic.ContainsKey("class"))
            {
                htmlAttributesDic["class"] += " " + cssClass + " ";
            }
            else
            {
                htmlAttributesDic.Add("class", cssClass);
            }
            return htmlAttributesDic;
        }

        public static RouteValueDictionary AddRouteValue(object originalRouteValues, object addedRouteValues)
        {
            var originalRouteValuesDic = ConvertRouteValuesToIDictionary(originalRouteValues);
            var addedRouteValuesDic = ConvertRouteValuesToIDictionary(addedRouteValues);

            var dic=originalRouteValuesDic.Concat(addedRouteValuesDic).GroupBy(o => o.Key).ToDictionary(o => o.Key, v => v.First().Value);

            var rdic=new RouteValueDictionary();
            foreach (var VARIABLE in dic)
            {
                rdic.Add(VARIABLE.Key,VARIABLE.Value);
            }

            return rdic;
        }

        private static string CreateMaterialIcon(string materialIconName)
        {
            var builderI = new TagBuilder("i");
            builderI.AddCssClass("material-icons");
            builderI.InnerHtml = materialIconName;
            return builderI.ToString();
        }

        private static MvcHtmlString CreateAButton( string value, string id, string materialIconName, object htmlAttributes, bool isSubmit = false, bool isLinkBtn = false)
        {
            // Create tag builder
            TagBuilder builder;
            if (isLinkBtn)
            {
                builder = new TagBuilder("a");
                isSubmit = false;
            }
            else
            {
                builder = new TagBuilder("button");
            }


            // Create valid id
            if (!string.IsNullOrEmpty(id))
            {
                builder.GenerateId(id);
            }

            // Add attributes
            builder.MergeAttributes(ConvertHtmlAttributesToIDictionary(htmlAttributes));

            builder.MergeAttribute("value", value);
            if (!string.IsNullOrEmpty(materialIconName))
            {
                builder.InnerHtml = CreateMaterialIcon(materialIconName);

            }
            if (isSubmit)
            {
                builder.MergeAttribute("type", "submit");
            }

            // Render tag
            return MvcHtmlString.Create(builder.ToString());
        }

        public static MvcHtmlString DisplayValueFor<TModel,TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression)
        {
            var value = ModelMetadata.FromLambdaExpression(expression, html.ViewData).Model;
            if (value is Enum)
            {
                return MvcHtmlString.Create(GetEnumPropertyValue( value as Enum));
            }
            return value!=null? html.DisplayFor(expression):MvcHtmlString.Empty;
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
                colBuilder.InnerHtml=tdContentDic[i].ToString();

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


        public static ButtonControlAjax Button(this AjaxHelper helper, string actionName, string controllerName, string updateTargetId, string loadingElementId)
        {
            return new ButtonControlAjax(helper, actionName, controllerName);
            //// ajax options
            //var ajaxOptions = new AjaxOptions
            //{
            //    HttpMethod = "POST",
            //    InsertionMode = InsertionMode.Replace,
            //    UpdateTargetId = updateTargetId,
            //    LoadingElementId = loadingElementId
            //};


            //if (isLinkBtn)
            //{

            //    string innerHtmlOrText = string.Empty;
            //    if (!string.IsNullOrEmpty(materialIconName))
            //    {
            //        innerHtmlOrText = string.Format( CreateMaterialIcon(materialIconName));
            //    }
            //    else
            //    {
            //        innerHtmlOrText = btnText ?? string.Empty;
            //    }

            //    // Reference:http://stackoverflow.com/questions/12008899/create-ajax-actionlink-with-html-elements-in-the-link-text
            //    var replacedText = Guid.NewGuid().ToString();
            //    var actionLink =helper.ActionLink(replacedText, actionName, controllerName, routeValues,ajaxOptions, htmlAttributes);
            //    return MvcHtmlString.Create(actionLink.ToString().Replace(replacedText, innerHtmlOrText));
            //}
            //else
            //{
            //   var form= helper.BeginForm( actionName,  controllerName, routeValues,ajaxOptions,htmlAttributes);
            //    var attributes = HtmlHelperExtension.ConvertHtmlAttributesToIDictionary(htmlAttributes);
            //    attributes.Add("id", id);
            //    var button= MvcHtmlString.Create(
            //        new ButtonControl().BtnText(btnText)
            //            .MaterialIcon(materialIconName)
            //            .Attributes(htmlAttributes).ToHtmlString());


            //    return MvcHtmlString.Create(form.ToString()+button.ToHtmlString()+"</form>");
        //}


        }
        public static AlertControl Alert(this HtmlHelper html, string text)
        {
            return new AlertControl(text);
        }

        public static AlertControl AlertFor<TModel, TTextProperty, TStyleProperty>(this HtmlHelper<TModel> html,Expression<Func<TModel, TTextProperty>> textExpression)
        {
            var text = (string)ModelMetadata.FromLambdaExpression(textExpression, html.ViewData).Model;

            return new AlertControl(text);
        }
    }

}