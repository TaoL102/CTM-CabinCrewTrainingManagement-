﻿using System.Web.Mvc;
using CTMCustomControlLib.CustomControls.Div;
using CTMCustomControlLib.Extensions;

namespace CTMCustomControlLib.CustomControls.TextBox
{
    public class TextBoxControl:CustomControlBase, ITextBoxProperty
    {

        public string Placeholder { get; set; }
        public string GoogleIcon { get; set; }
        public string GlyphIcon { get; set; }
        public string LabelText { get; set; }

        protected override string Render()
        {

            TagBuilder input=new TagBuilder("input");
            TagBuilder label =null;
            TagBuilder googleIcon = null;
            TagBuilder glyphIcon = null;

            input.MergeAttribute("type","text");
            input.AddCssClass("form-control");
            input.MergeAttribute("placeholder",Placeholder);
            input.GenerateId(Id);

            if (LabelText != null)
            {
                label = new TagBuilder("label");
                label.MergeAttribute("for", Id);
                label.SetInnerText(LabelText);
            }

            if (GoogleIcon!=null)
            {
                googleIcon=new TagBuilder("span");
                googleIcon.AddCssClass("input-group-addon");
                TagBuilder i=new TagBuilder("i");
                i.AddCssClass("material-icons");
                i.SetInnerText(GoogleIcon);
            }

            if (GlyphIcon != null)
            {
                glyphIcon = new TagBuilder("span");
                glyphIcon.AddCssClass("input-group-addon");
                TagBuilder i = new TagBuilder("i");
                i.AddCssClass("glyphicon");
                i.AddCssClass(GoogleIcon);
            }

            var wrapper = new DivControl(
                label?.ToString()
                +googleIcon
                +glyphIcon+input)
                .AddCssClass("form-group").ToHtmlString();

            return wrapper;
        }

    }
}
