using CTMLib.CustomControls;
using CTMLib.CustomControls.Alert;
using CTMLib.CustomControls.Button;

namespace CTMLib.Extensions
{
    public static class CustomControlExtension
    {
        #region IcustomControlOptions
        public static T SetId<T>(this T obj, string id) where T : ICustomControlOptions
        {
            obj.Id = id;
            return obj;
        }
        public static T SetAttributes<T>(this T obj,  object htmlAttributes) where T:ICustomControlOptions
        {
            obj.HtmlAttributes = HtmlHelperExtension.ConvertHtmlAttributesToIDictionary(htmlAttributes);
            return obj;
        }

        public static T SetRouteValues<T>(this T obj, object routeValues) where T : ICustomControlOptions
        {
            obj.RouteValues = HtmlHelperExtension.ConvertRouteValuesToIDictionary(routeValues);
            return obj;
        }
        public static T AddCssClass<T>(this T obj, string cssClass) where T : ICustomControlOptions
        {
            obj.HtmlAttributes = HtmlHelperExtension.AddCssClass(obj.HtmlAttributes,cssClass);
            return obj;
        }

        public static T RemoveCssClass<T>(this T obj, string cssClass) where T : ICustomControlOptions
        {
            obj.HtmlAttributes = HtmlHelperExtension.RemoveCssClass(obj.HtmlAttributes,cssClass);
            return obj;
        }

        public static T RemoveAllCssClass<T>(this T obj) where T : ICustomControlOptions
        {
            obj.HtmlAttributes = HtmlHelperExtension.RemoveAllCssClass(obj.HtmlAttributes);
            return obj;
        }

        public static T MergeAttribute<T>(this T obj, string key,string value) where T : ICustomControlOptions
        {
            obj.HtmlAttributes = HtmlHelperExtension.MergeAttribute(obj.HtmlAttributes, key,value);
            return obj;
        }
        public static T MergeAttributes<T>(this T obj, object htmlAttributes) where T : ICustomControlOptions
        {
            obj.HtmlAttributes = HtmlHelperExtension.MergeAttributes(obj.HtmlAttributes, htmlAttributes);
            return obj;
        }



        public static T Hide<T>(this T obj) where T : ICustomControlOptions
        {
            obj.HtmlAttributes = HtmlHelperExtension.MergeAttributes(obj.HtmlAttributes, new {style="display:none"});
            return obj;

        }

        #endregion

        #region IColorOptions
        public static T SetColor<T>(this T obj, ColorOptions colorOption) where T : IColorOptions
        {
            obj.BackgroundColor = colorOption;
            return obj;
        }

        #endregion

        #region ISizeOptions
        public static T SetSize<T>(this T obj, SizeOptions sizeOption) where T : ISizeOptions
        {
            obj.Size = sizeOption;
            return obj;
        }
        #endregion

        #region IButtonControl

        public static T SetText<T>(this T obj, string text) where T : IButtonControlBase
        {
            obj.Text = text;
            return obj;
        }
        public static T SetMaterialIcon<T>(this T obj, string materialIcon) where T : IButtonControlBase
        {
            obj.MaterialIcon = materialIcon;
            return obj;
        }
        public static T IsLinkBtn<T>(this T obj, bool isLinkBtn) where T : IButtonControlBase
        {
            obj.IsLinkBtn = isLinkBtn;
            return obj;
        }
        public static T IsSubmitBtn<T>(this T obj, bool isSubmitBtn) where T : IButtonControlBase
        {
            obj.IsSubmitBtn = isSubmitBtn;
            return obj;
        }

        #endregion

        #region IDialogueOptions

        public static T HasCloseBtn<T>(this T obj, bool hasCloseBtn) where T : IDialogueOptions
        {
            obj.HasCloseBtn = hasCloseBtn;
            return obj;
        }

        #endregion

        #region IModalOptions
        public static T SetBodyHtml<T>(this T obj, string bodyId,string bodyHtml) where T : IModalOptions
        {
            obj.BodyId = bodyId;
            obj.BodyInnerHtml = bodyHtml;
            return obj;
        }
        public static T SetFooterHtml<T>(this T obj,string footerHtml) where T : IModalOptions
        {
            obj.BodyInnerHtml = footerHtml;
            return obj;
        }

        public static T SetFooterYesBtn<T>(this T obj, string footerHtml) where T : IModalOptions
        {
            var yesBtn = new ButtonControl().SetText("Confirm").SetColor(ColorOptions.Danger);
            obj.BodyInnerHtml = footerHtml;
            return obj;
        }

        #endregion

        #region IAjaxOptions
        public static T SetUpdateTargetId<T>(this T obj, string updateTargetId) where T : IAjaxOptions
        {
            obj.UpdateTargetId = updateTargetId;
            return obj;
        }
        public static T SetLoadingElementId<T>(this T obj, string loadingElementId) where T : IAjaxOptions
        {
            obj.LoadingElementId = loadingElementId;
            return obj;
        }
        public static T SetOnSuccessFun<T>(this T obj, string onSuccessFun) where T : IAjaxOptions
        {
            obj.OnSuccessFun = onSuccessFun;
            return obj;
        }
        #endregion

        #region ITextBoxOptions

        public static T SetLabelText<T>(this T obj, string labelText) where T : ITextBoxOptions
        {
            obj.LabelText = labelText;
            return obj;
        }
        public static T SetPlaceholder<T>(this T obj, string placeholder) where T : ITextBoxOptions
        {
            obj.Placeholder = placeholder;
            return obj;
        }
        public static T SetGoogleIcon<T>(this T obj, string googleIcon) where T : ITextBoxOptions
        {
            obj.GoogleIcon = googleIcon;
            return obj;
        }
        public static T SetGlyphIcon<T>(this T obj, string glyphIcon) where T : ITextBoxOptions
        {
            obj.GlyphIcon = glyphIcon;
            return obj;
        }

        #endregion
    }
}