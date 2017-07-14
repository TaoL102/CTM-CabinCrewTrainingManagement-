using System.Web.Mvc;
using CTMCustomControlLib.Helpers;

namespace CTMCustomControlLib.CustomControls.Label
{
    public class LabelControl:CustomControlBase,IColorProperty
    {

        protected override string Render()
        {
            // Label
            TagBuilder label = new TagBuilder("label");
            label.MergeAttribute("for", Id);

            // HtmlAttributes
            label.MergeAttributes(HtmlAttributes);

            // Style: SetColor 
            label.AddCssClass(CssHelper<LabelControl>.ConvertToCss(BackgroundColor));

            return label.ToString();
        }

        public ColorOptions BackgroundColor { get; set; }
    }
}
