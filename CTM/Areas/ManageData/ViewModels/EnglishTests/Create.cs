using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CTM.Codes.Attributes;
using CTMLib.Models;
using CTMLib.Resources;

namespace CTM.Areas.ManageData.ViewModels.EnglishTests
{
    public class Create : Model<EnglishTest>
    {
        private DateTime date;

        public Create()
        {
            Date = DateTime.Today;
        }
        
        [IsCabinCrew]
        [Required]
        [Display(Name = "CabinCrewName", ResourceType = typeof(ConstModels))]
        public string CCName { get; set; }

        [Required]
        [EnumDataType(typeof(EnglishTestType))]
        [Display(Name = "SubCategory", ResourceType = typeof(ConstModels))]
        public EnglishTestType Type { get; set; }

        [Required]
        [Display(Name = "Grade", ResourceType = typeof(ConstModels))]
        public string Grade { get; set; }

        [Required]
        [Display(Name = "Category", ResourceType = typeof(ConstModels))]
        public string CategoryID { get; set; }
        public SelectList CategoryList { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date", ResourceType = typeof(ConstModels))]
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
    }
}