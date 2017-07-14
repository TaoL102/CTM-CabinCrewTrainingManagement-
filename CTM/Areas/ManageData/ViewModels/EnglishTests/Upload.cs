using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using CTM.Codes.Interfaces;
using CTM.Models;
using CTMLocalizationLib.Resources;

namespace CTM.Areas.ManageData.ViewModels.EnglishTests
{
    public class Upload :IUpload, IEnglishTest
    {
        private DateTime date;

        public Upload()
        {
            UploadRecordID=Guid.NewGuid().ToString();
            Date=DateTime.Today;
        }

        [Required]
        [Display(Name = "Category", ResourceType = typeof(ConstModels))]
        public string CategoryID { get; set; }
        public SelectList CategoryList { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date", ResourceType = typeof(ConstModels))]
        [Required]
        public DateTime Date
        {
            get
            {
                return date.ToLocalTime();
            }
            set
            {
                date = value.ToUniversalTime().Date;
            }
        }
        [Required]
        [Display(Name = "File", ResourceType = typeof(ConstModels))]
        public string File { get; set; }

        public string UploadRecordID { get; set; }

    }
}
