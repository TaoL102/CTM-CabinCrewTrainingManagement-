using System.Web.Mvc;
using CTMLib.Helpers;

namespace CTMLib.CustomControls.Button
{
    public class ButtonControl : ButtonControlBase
    {
        protected override string Render()
        {
            // Create tag builder
            TagBuilder builder;
            if (IsLinkBtn)
            {
                builder = new TagBuilder("a");
                IsSubmitBtn = false;
            }
            else
            {
                builder = new TagBuilder("button");
            }

            // attributes
            if (HtmlAttributes.ContainsKey("id"))
            {
                if (HtmlAttributes["id"] != null)
                {
                    builder.GenerateId(HtmlAttributes["id"].ToString());
                }
                HtmlAttributes.Remove("id");
            }
            if (!string.IsNullOrEmpty(Text))
            {
                HtmlAttributes.Add("value", Text);
            }
            if (IsSubmitBtn)
            {
                HtmlAttributes.Add("type", "submit");
            }
            builder.MergeAttributes(HtmlAttributes);

            // Material Icon
            if (!string.IsNullOrEmpty(MaterialIcon))
            {
                builder.InnerHtml = RenderMaterialIcon(MaterialIcon);
            }

            // Style: SetColor & SetSize
            builder.AddCssClass(CssHelper<ButtonControl>.ControlTypeAbbr);
            builder.AddCssClass(CssHelper<ButtonControl>.ConvertToCss(BackgroundColor));
            builder.AddCssClass(CssHelper<ButtonControl>.ConvertToCss(Size));

            return builder.ToString();
        }
    }
}