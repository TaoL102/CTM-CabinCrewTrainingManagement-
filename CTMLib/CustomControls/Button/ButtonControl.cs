﻿using System.Web.Mvc;
using CTMCustomControlLib.Helpers;

namespace CTMCustomControlLib.CustomControls.Button
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


            // Id
            builder.GenerateId(Id);

            // Text
            builder.SetInnerText(Text);

            // IsSubmit
            HtmlAttributes.Add("type", IsSubmitBtn?"submit":"button");

            // Material Icon
            if (!string.IsNullOrEmpty(MaterialIcon))
            {
                builder.InnerHtml = RenderMaterialIcon(MaterialIcon);
            }

            // Merge Attributes
            builder.MergeAttributes(HtmlAttributes);

            // Style: SetColor & SetSize
            builder.AddCssClass(CssHelper<ButtonControl>.ControlTypeAbbr);
            builder.AddCssClass(CssHelper<ButtonControl>.ConvertToCss(BackgroundColor));
            builder.AddCssClass(CssHelper<ButtonControl>.ConvertToCss(Size));

            return builder.ToString();
        }
    }
}