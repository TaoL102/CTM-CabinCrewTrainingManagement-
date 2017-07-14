using System.Web.Mvc;

namespace CTMCustomControlLib.CustomControls.Div
{
    public class DivControl:CustomControlBase
    {
        private readonly string _textOrHtml;
        public DivControl(string textOrHtml=null)
        {
            this._textOrHtml = textOrHtml;
        }
        protected override string Render()
        {
            TagBuilder builder=new TagBuilder("div");

            // Id
            builder.GenerateId(Id);
            builder.MergeAttributes(HtmlAttributes);
            builder.InnerHtml = _textOrHtml;

            return builder.ToString();
        }
    }
}
