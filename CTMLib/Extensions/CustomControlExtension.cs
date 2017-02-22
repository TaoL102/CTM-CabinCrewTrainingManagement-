using CTMLib.CustomControls;
using CTMLib.CustomControls.Alert;
using CTMLib.CustomControls.Button;

namespace CTMLib.Extensions
{
    public static class CustomControlExtension
    {
        #region IcustomControlOptions

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

        #endregion

        #region IColorOptions
        public static T SetColor<T>(this T obj, ColorOptions colorOption) where T : IColorOptions
        {
            obj.BackgroundColor = colorOption;
            return obj;
        }

        #endregion

        #region ISizeOptions
        public static T SetColor<T>(this T obj, SizeOptions sizeOption) where T : ISizeOptions
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
    }
}