using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using CTMLib.CustomControls.Div;
using CTMLib.Extensions;
using Microsoft.Ajax.Utilities;

namespace CTMLib.CustomControls.Table
{
    public class TableControl : CustomControlBase
    {
        private readonly string[] _header;
        private readonly string[][] _rows;
        private readonly Dictionary<string, string[]> _rowsWithId;
        private readonly Dictionary<string, Dictionary<string, string>> _rowsWithIdAndTrWithNameAttr;
        public TableControl(string[] header, string[][] rows)
        {
            _header = header;
            _rows = rows;
        }

        public TableControl(string[] header, Dictionary<string, string[]> rowsWithId)
        {
            _header = header;
            _rowsWithId = rowsWithId;
        }

        public TableControl(string[] header, Dictionary<string, Dictionary<string, string>> rowsWithIdAndTrWithNameAttr)
        {
            _header = header;
            _rowsWithIdAndTrWithNameAttr = rowsWithIdAndTrWithNameAttr;
        }

        protected override string Render()
        {

            // Table
            TagBuilder table = new TagBuilder("table");
            table.AddCssClass("table");
            table.AddCssClass("table-hover");

            // Header
            TagBuilder header = _header != null ? null : new TagBuilder("thead")
            {
                InnerHtml = new HeaderControl(_header).ToHtmlString()
            };

            // Body
            TagBuilder body = new TagBuilder("tbody");

            _rows?.ForEach(o =>
            {
                body.InnerHtml += new RowControl(o);
            });
            _rowsWithId?.ForEach(o =>
            {
                body.InnerHtml += new RowControl(o.Value).SetId(o.Key);
            });
            _rowsWithIdAndTrWithNameAttr?.ForEach(o =>
            {
                body.InnerHtml += new RowControl(o.Value).SetId(o.Key);
            });


            table.InnerHtml = header + body.ToString();

            // Container
            var container = new DivControl(table.ToString())
                .AddCssClass("table-responsive")
                .MergeAttributes(HtmlAttributes);

            return container.ToHtmlString();
        }
    }
}
