using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using CTMLib.Resources;

namespace CTM.Areas.ManageData.ViewModels.EnglishTests
{
    public class Upload
    {
        public Upload()
        {
            UploadRecordID=Guid.NewGuid().ToString();
            Date=DateTime.UtcNow;
        }

        [Required]
        [Display(Name = "Category", ResourceType = typeof(ConstModels))]
        public string CategoryID { get; set; }
        public SelectList CategoryList { get; set; }

        [Display(Name = "Date", ResourceType = typeof(ConstModels))]
        [Required]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "File", ResourceType = typeof(ConstModels))]
        public string File { get; set; }

        public string UploadRecordID { get; set; }

    }
}
