using CTMCustomControlLib.CustomControls;
using CTMCustomControlLib.CustomControls.Button;

namespace CTMCustomControlLib.Extensions
{
    public static class CustomControlExtension
    {
        #region IcustomControlOptions
        public static T SetId<T>(this T obj, string id) where T : ICustomControlBaseProperty
        {
            obj.Id = id;
            return obj;
        }
        public static T SetAttributes<T>(this T obj,  object htmlAttributes) where T:ICustomControlBaseProperty
        {
            obj.HtmlAttributes = HtmlHelperExtension.ConvertHtmlAttributesToIDictionary(htmlAttributes);
            return obj;
        }

        public static T SetRouteValues<T>(this T obj, object routeValues) where T : ICustomControlBaseProperty
        {
            obj.RouteValues = HtmlHelperExtension.ConvertRouteValuesToIDictionary(routeValues);
            return obj;
        }
        public static T AddCssClass<T>(this T obj, string cssClass) where T : ICustomControlBaseProperty
        {
            obj.HtmlAttributes = HtmlHelperExtension.AddCssClass(obj.HtmlAttributes,cssClass);
            return obj;
        }

        public static T RemoveCssClass<T>(this T obj, string cssClass) where T : ICustomControlBaseProperty
        {
            obj.HtmlAttributes = HtmlHelperExtension.RemoveCssClass(obj.HtmlAttributes,cssClass);
            return obj;
        }

        public static T RemoveAllCssClass<T>(this T obj) where T : ICustomControlBaseProperty
        {
            obj.HtmlAttributes = HtmlHelperExtension.RemoveAllCssClass(obj.HtmlAttributes);
            return obj;
        }

        public static T MergeAttribute<T>(this T obj, string key,string value) where T : ICustomControlBaseProperty
        {
            obj.HtmlAttributes = HtmlHelperExtension.MergeAttribute(obj.HtmlAttributes, key,value);
            return obj;
        }
        public static T MergeAttributes<T>(this T obj, object htmlAttributes) where T : ICustomControlBaseProperty
        {
            obj.HtmlAttributes = HtmlHelperExtension.MergeAttributes(obj.HtmlAttributes, htmlAttributes);
            return obj;
        }



        public static T Hide<T>(this T obj) where T : ICustomControlBaseProperty
        {
            obj.HtmlAttributes = HtmlHelperExtension.MergeAttribute(obj.HtmlAttributes, "style","display:none");
            return obj;

        }

        #endregion

        #region IColorProperty
        public static T SetColor<T>(this T obj, ColorOptions colorOption) where T : IColorProperty
        {
            obj.BackgroundColor = colorOption;
            return obj;
        }

        #endregion

        #region ISizeProperty
        public static T SetSize<T>(this T obj, SizeOptions sizeOption) where T : ISizeProperty
        {
            obj.Size = sizeOption;
            return obj;
        }
        #endregion

        #region IButtonControl

        public static T SetText<T>(this T obj, string text) where T : IButtonControl
        {
            obj.Text = text;
            return obj;
        }
        public static T SetMaterialIcon<T>(this T obj, string materialIcon) where T : IButtonControl
        {
            obj.MaterialIcon = materialIcon;
            return obj;
        }
        public static T IsLinkBtn<T>(this T obj, bool isLinkBtn) where T : IButtonControl
        {
            obj.IsLinkBtn = isLinkBtn;
            return obj;
        }
        public static T IsSubmitBtn<T>(this T obj, bool isSubmitBtn) where T : IButtonControl
        {
            obj.IsSubmitBtn = isSubmitBtn;
            return obj;
        }

        #endregion

        #region IDialogueProperty

        public static T HasCloseBtn<T>(this T obj, bool hasCloseBtn) where T : IDialogueProperty
        {
            obj.HasCloseBtn = hasCloseBtn;
            return obj;
        }

        #endregion

        #region IModalProperty
        public static T SetBodyHtml<T>(this T obj, string bodyId,string bodyHtml) where T : IModalProperty
        {
            obj.BodyId = bodyId;
            obj.BodyInnerHtml = bodyHtml;
            return obj;
        }
        public static T SetFooterHtml<T>(this T obj,string footerHtml) where T : IModalProperty
        {
            obj.BodyInnerHtml = footerHtml;
            return obj;
        }

        public static T SetFooterYesBtn<T>(this T obj, string footerHtml) where T : IModalProperty
        {
            var yesBtn = new ButtonControl().SetText("Confirm").SetColor(ColorOptions.Danger);
            obj.BodyInnerHtml = footerHtml;
            return obj;
        }

        #endregion

        #region IAjaxProperty
        public static T SetUpdateTargetId<T>(this T obj, string updateTargetId) where T : IAjaxProperty
        {
            obj.UpdateTargetId = updateTargetId;
            return obj;
        }
        public static T SetLoadingElementId<T>(this T obj, string loadingElementId) where T : IAjaxProperty
        {
            obj.LoadingElementId = loadingElementId;
            return obj;
        }
        public static T SetOnSuccessFun<T>(this T obj, string onSuccessFun) where T : IAjaxProperty
        {
            obj.OnSuccessFun = onSuccessFun;
            return obj;
        }
        #endregion

        #region ITextBoxProperty

        public static T SetLabelText<T>(this T obj, string labelText) where T : ITextBoxProperty
        {
            obj.LabelText = labelText;
            return obj;
        }
        public static T SetPlaceholder<T>(this T obj, string placeholder) where T : ITextBoxProperty
        {
            obj.Placeholder = placeholder;
            return obj;
        }
        public static T SetGoogleIcon<T>(this T obj, string googleIcon) where T : ITextBoxProperty
        {
            obj.GoogleIcon = googleIcon;
            return obj;
        }
        public static T SetGlyphIcon<T>(this T obj, string glyphIcon) where T : ITextBoxProperty
        {
            obj.GlyphIcon = glyphIcon;
            return obj;
        }

        #endregion
    }
}