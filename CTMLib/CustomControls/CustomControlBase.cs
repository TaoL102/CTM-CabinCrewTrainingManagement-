using System.Collections.Generic;
using System.Web.Routing;

namespace CTMCustomControlLib.CustomControls
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
