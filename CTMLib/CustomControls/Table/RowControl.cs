using System.Collections.Generic;
using System.Web.Mvc;
using WebGrease.Css.Extensions;

namespace CTMCustomControlLib.CustomControls.Table
{
    public class RowControl : CustomControlBase
    {
        private readonly string[] _innerHtml;
        private readonly Dictionary<string, string> _keyAndInnerHtml;
        public RowControl(Dictionary<string, string> keyAndInnerHtml)
        {
            _keyAndInnerHtml = keyAndInnerHtml;
        }

        public RowControl(params string[] innerHtml)
        {
            _innerHtml = innerHtml;
        }

        protected override string Render()
        {
            TagBuilder builder = new TagBuilder("tr");
            builder.MergeAttribute("id", Id);

            _innerHtml?.ForEach(o =>
            {
                TagBuilder td = new TagBuilder("td")
                {
                    InnerHtml = o
                };
                builder.InnerHtml += td;
            });

            _keyAndInnerHtml?.ForEach(o =>
            {
                TagBuilder td = new TagBuilder("td")
                {
                    InnerHtml = o.Value
                };
                td.MergeAttribute("name", o.Key);
                builder.InnerHtml += td;
            });

            return builder.ToString();
        }
    }
}
