using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace CTMLib.CustomControls
{
   public abstract class CustomControlBase: ICustomControlBaseProperty
   {
        public Dictionary<string, object> HtmlAttributes { get; set; }
        public RouteValueDictionary RouteValues { get; set; }
        public string Id { get; set; }

        public override string ToString()
        {
            return Render();
        }

        public string ToHtmlString()
        {
            return ToString();
        }

        protected abstract string Render();
   }


}
