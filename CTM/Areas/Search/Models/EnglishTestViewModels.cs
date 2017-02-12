using System;
using System.ComponentModel.DataAnnotations;
using CTM.Models;

namespace CTM.Areas.Search.Models
{
    public class EnglishTestViewModels
    {
        public class DisplayEnglishTestsViewModel
        {

            public string ID { get; set; }

            public string CabinCrewID { get; set; }

            [Display(Name = "CabinCrewName", ResourceType = typeof(Resources.Models.ConstModels))]
            public string CabinCrewName { get; set; }

            [Display(Name = "Category", ResourceType = typeof(Resources.Models.ConstModels))]
            public string CategoryName { get; set; }

            [EnumDataType(typeof(EnglishTestType))]
            [Display(Name = "EnglishTestType", ResourceType = typeof(Resources.Models.ConstModels))]
            public EnglishTestType Type { get; set; }

            [Display(Name = "Grade", ResourceType = typeof(Resources.Models.ConstModels))]
            public string Grade { get; set; }

            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            [DataType(DataType.Date)]
            [Display(Name = "Date", ResourceType = typeof(Resources.Models.ConstModels))]
            public DateTime? Date { get; set; }




        }

        public class DisplayEnglishTestsIsLatestViewModel
        {


            public string CabinCrewID { get; set; }

            [Display(Name = "CabinCrewName", ResourceType = typeof(Resources.Models.ConstModels))]
            public string CabinCrewName { get; set; }

            [Display(Name = "Grade", ResourceType = typeof(Resources.Models.ConstModels))]
            public string CabinAnnoucementGrade { get; set; }

            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            [DataType(DataType.Date)]
            [Display(Name = "Date", ResourceType = typeof(Resources.Models.ConstModels))]
            public DateTime? CabinAnnoucementDate { get; set; }

            [Display(Name = "Category", ResourceType = typeof(Resources.Models.ConstModels))]
            public string CabinAnnoucementCategoryName { get; set; }

            [Display(Name = "Grade", ResourceType = typeof(Resources.Models.ConstModels))]
            public string SpokenSkillGrade { get; set; }

            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            [DataType(DataType.Date)]
            [Display(Name = "Date", ResourceType = typeof(Resources.Models.ConstModels))]
            public DateTime? SpokenSkillDate { get; set; }

            [Display(Name = "Category", ResourceType = typeof(Resources.Models.ConstModels))]
            public string SpokenSkillCategoryName { get; set; }





        }
    }
}