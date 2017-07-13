using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using CTMLib.CustomControls.Button;
using CTMLib.Helpers;

namespace CTMLib.CustomControls.Label
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
