using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CTM.Codes.Interfaces;
using CTM.Models;
using CTMLocalizationLib.Resources;

namespace CTM.Areas.ManageData.ViewModels
{
    public class UploadTemplateBase 
    {
        [Display(Name = "CabinCrewID", ResourceType = typeof(ConstModels))]
        public string CabinCrewID { get; set; }

        [Display(Name = "CabinCrewName", ResourceType = typeof(ConstModels))]
        public string CabinCrewName { get; set; }

    }
}