using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CTM.Codes.Attributes;

namespace CTM.Areas.Search.ViewModels.EnglishTests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using CTMLib.Models;
    using CTMLib.Resources;

    public class Search:Model<EnglishTest>, ISearchViewModel
    {
        [IsCabinCrew]
        [Display(Name = "CabinCrewName", ResourceType = typeof(ConstModels))]
        public string CCName { get; set; }

        [Display(Name = "Category", ResourceType = typeof(ConstModels))]
        public string CategoryID { get; set; }
        public SelectList CateforyList { get; set; }

        [Display(Name = "FromDate", ResourceType = typeof(ConstModels))]
        public DateTime? FromDate { get; set; }

        [Display(Name = "ToDate", ResourceType = typeof(ConstModels))]
        public DateTime? ToDate { get; set; }

        public string UploadRecordID { get; set; }
        public int? Page { get; set; }

        [Display(Name = "IsLatestResultOnly", ResourceType = typeof(ConstModels))]
        public bool IsLatest { get; set; }
    }
}
