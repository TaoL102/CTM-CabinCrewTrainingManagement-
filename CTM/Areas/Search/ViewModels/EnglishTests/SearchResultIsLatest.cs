using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CTMLib.Models;
using CTMLib.Resources;

namespace CTM.Areas.Search.ViewModels.EnglishTests
{
    public class SearchResultIsLatest: Model<EnglishTest>
    {

        public string CabinCrewID { get; set; }

        [Display(Name = "CabinCrewName", ResourceType = typeof(ConstModels))]
        public string CabinCrewName { get; set; }

        [Display(Name = "Grade", ResourceType = typeof(ConstModels))]
        public string CabinAnnoucementGrade { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = "Date", ResourceType = typeof(ConstModels))]
        public DateTime? CabinAnnoucementDate { get; set; }

        [Display(Name = "Category", ResourceType = typeof(ConstModels))]
        public string CabinAnnoucementCategoryName { get; set; }

        [Display(Name = "Grade", ResourceType = typeof(ConstModels))]
        public string SpokenSkillGrade { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = "Date", ResourceType = typeof(ConstModels))]
        public DateTime? SpokenSkillDate { get; set; }

        [Display(Name = "Category", ResourceType = typeof(ConstModels))]
        public string SpokenSkillCategoryName { get; set; }
    }
}