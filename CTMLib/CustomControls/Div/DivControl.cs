using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CTMLib.CustomControls.Div
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
