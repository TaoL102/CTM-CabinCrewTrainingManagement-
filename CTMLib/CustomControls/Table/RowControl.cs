using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CTMLib.CustomControls.Table
{
    public class RowControl:CustomControlBase
    {
        private readonly string[] _innerHtmls;
        public RowControl(params string[] innerHtmls)
        {
            _innerHtmls = innerHtmls;
        }

        protected override string Render()
        {
            TagBuilder builder=new TagBuilder("tr");
            builder.GenerateId(Id);
            for (int i = 0; i < _innerHtmls.Length; i++)
            {
                TagBuilder td = new TagBuilder("td");
                td.InnerHtml = _innerHtmls[i];
                builder.InnerHtml += td;
            }
            return builder.ToString();
        }
    }
}
