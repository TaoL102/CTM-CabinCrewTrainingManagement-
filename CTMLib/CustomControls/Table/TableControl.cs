using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using CTMLib.CustomControls.Div;
using CTMLib.Extensions;

namespace CTMLib.CustomControls.Table
{
    public class TableControl:CustomControlBase
    {
        private readonly string[] _header;
        private readonly string[][] _rows;
        private readonly Dictionary<string, string[]> _rowsWithId;
        public TableControl(string[] header,string[][] rows)
        {
            _header = header;
            _rows = rows;
        }

        public TableControl(string[] header,Dictionary<string,string[]> rowsWithId)
        {
            _header = header;
            _rowsWithId = rowsWithId;
        }
        protected override string Render()
        {
            
            // Table
            TagBuilder table=new TagBuilder("table");
            table.AddCssClass("table");
            table.AddCssClass("table-hover");

            // Header
            TagBuilder header = new TagBuilder("thead")
            {
                InnerHtml = new HeaderControl(_header).ToHtmlString()
            };

            // Body
            TagBuilder body=new TagBuilder("tbody");
            if (_rowsWithId==null)
            {
                for (int i = 0; i < _rows.Length; i++)
                {
                    body.InnerHtml += new RowControl(_rows[i]);
                }
            }
            else
            {
                foreach (var row in _rowsWithId)
                {
                    body.InnerHtml += new RowControl(row.Value).SetId(row.Key);
                }
            }

            table.InnerHtml = header + body.ToString();

            // Container
            var container = new DivControl(table.ToString())
                .AddCssClass("table-responsive");

            return container.ToHtmlString();
        }
    }
}
