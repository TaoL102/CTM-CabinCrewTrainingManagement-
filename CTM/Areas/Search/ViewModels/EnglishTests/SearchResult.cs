using System;
using System.ComponentModel.DataAnnotations;
using CTM.Models;
using CTMLocalizationLib.Resources;
using CTM.Codes.Interfaces;

namespace CTM.Areas.Search.ViewModels.EnglishTests
{
    public class SearchResult: IEnglishTest, ISearchResultModel
    {
        public string ID { get; set; }

        public string CabinCrewID { get; set; }

        [Display(Name = "CabinCrewName", ResourceType = typeof(ConstModels))]
        public string CabinCrewName { get; set; }

        [Display(Name = "Category", ResourceType = typeof(ConstModels))]
        public string CategoryName { get; set; }

        [EnumDataType(typeof(EnglishTestType))]
        [Display(Name = "EnglishTestType", ResourceType = typeof(ConstModels))]
        public EnglishTestType Type { get; set; }

        [Display(Name = "Grade", ResourceType = typeof(ConstModels))]
        public string Grade { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date", ResourceType = typeof(ConstModels))]
        public DateTime? Date { get; set; }

    }
}