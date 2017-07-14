using System.Web.Mvc;

namespace CTMCustomControlLib.CustomControls.Table
{
    public class HeaderControl:CustomControlBase
    {
        private readonly string[] _innerHtmls;
        public HeaderControl(params string[] innerHtmls)
        {
            _innerHtmls = innerHtmls;
        }

        protected override string Render()
        {

            TagBuilder builder=new TagBuilder("tr");
            builder.GenerateId(Id);
            for (int i = 0; i < _innerHtmls.Length; i++)
            {
                TagBuilder th = new TagBuilder("th");
                th.InnerHtml = _innerHtmls[i];
                builder.InnerHtml += th;
            }
            return builder.ToString();
        }
    }
}
