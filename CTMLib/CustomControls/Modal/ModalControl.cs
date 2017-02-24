using System.Web.Mvc;
using CTMLib.Helpers;

namespace CTMLib.CustomControls.Modal
{
    public class ModalControl:CustomControlBase, IModalOptions,ISizeOptions
    {
        private readonly string _title;

        public SizeOptions Size { get; set; }
        public string BodyInnerHtml { get; set; }
        public string BodyId { get; set; }
        public string FooterInnerHtml { get; set; }

        public ModalControl(string id,string title)
        {
            Id = id;
            _title = title;
        }
        protected override string Render()
        {
            // Create tag builder
            TagBuilder builder1=new TagBuilder("div");
            TagBuilder builder2 = new TagBuilder("div");
            TagBuilder builder3 = new TagBuilder("div");
            TagBuilder builder4 = new TagBuilder("div");
            TagBuilder builder5 = new TagBuilder("div");
            TagBuilder builder6 = new TagBuilder("div");

            builder1.GenerateId(Id);
            builder1.AddCssClass("modal");
            builder1.AddCssClass("fade");
            builder1.MergeAttribute("role","dialog");

            builder2.AddCssClass("modal-dialog");
            builder2.AddCssClass(CssHelper<ModalControl>.ConvertToCss(Size));

            builder3.AddCssClass("modal-content");

            builder4.AddCssClass("modal-header");
            TagBuilder buiderTitle = new TagBuilder("h4");
            buiderTitle.AddCssClass("modal-title");
            buiderTitle.SetInnerText(_title);
            TagBuilder buiderCloseBtn = new TagBuilder("button");
            buiderCloseBtn.AddCssClass("close");
            buiderCloseBtn.SetInnerText("×");
            buiderCloseBtn.MergeAttribute("data-dismiss", "modal");
            builder4.InnerHtml = buiderCloseBtn+ buiderTitle.ToString() ;

            builder5.AddCssClass("modal-body");
            builder5.GenerateId(BodyId);
            builder5.InnerHtml =BodyInnerHtml;

            if (FooterInnerHtml!=null)
            {
                builder6.AddCssClass("modal-footer");
                builder6.InnerHtml = FooterInnerHtml;
            }

            // Wrap
            builder3.InnerHtml = builder4.ToString()+builder5+builder6;
            builder2.InnerHtml = builder3.ToString();
            builder1.InnerHtml = builder2.ToString();

            return builder1.ToString();
        }

    }
}
