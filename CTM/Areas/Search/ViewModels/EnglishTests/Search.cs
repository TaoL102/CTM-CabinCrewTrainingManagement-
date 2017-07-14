using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using CTM.Codes.Attributes;
using CTM.Codes.Interfaces;
using CTM.Models;
using CTMLocalizationLib.Resources;

namespace CTM.Areas.Search.ViewModels.EnglishTests
{
    public class Search: ISearchViewModel,IEnglishTest
    {
        [IsCabinCrew]
        [Display(Name = "CabinCrewName", ResourceType = typeof(ConstModels))]
        public string CCName { get; set; }

        [Display(Name = "Category", ResourceType = typeof(ConstModels))]
        public string CategoryID { get; set; }
        public SelectList CategoryList { get; set; }

        [Display(Name = "FromDate", ResourceType = typeof(ConstModels))]
        public DateTime? FromDate { get; set; }

        [Display(Name = "ToDate", ResourceType = typeof(ConstModels))]
        public DateTime? ToDate { get; set; }

        public string UploadRecordID { get; set; }
        public int? Page { get; set; }

        [Display(Name = "IsLatestResultOnly", ResourceType = typeof(ConstModels))]
        public bool IsLatest { get; set; }

        public bool IsDownload { get; set; }
    }
}
