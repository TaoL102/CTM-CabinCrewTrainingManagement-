using System.Web.Mvc;
using CTMCustomControlLib.CustomControls.Label;
using CTMCustomControlLib.Extensions;

namespace CTMCustomControlLib.CustomControls.CheckBox
{
    public class CheckBoxControl : CustomControlBase,IColorProperty
    {
        protected override string Render()
        {
            // Create tag builder
            TagBuilder checkBox;

            checkBox = new TagBuilder("input");
            checkBox.MergeAttribute("name",Id);
            checkBox.MergeAttribute("type","checkbox");
            checkBox.MergeAttribute("value", "true");
            checkBox.MergeAttribute("checked","checked");
            checkBox.GenerateId(Id);

           // Merge Attributes
            checkBox.MergeAttributes(HtmlAttributes);

            // Label
            var label=new LabelControl().SetId(Id);

            // Style: SetColor & SetSize
            label.SetColor(BackgroundColor);

            return checkBox.ToString()+label;
        }

        public ColorOptions BackgroundColor { get; set; }
    }
}