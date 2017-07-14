using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CTM.Codes.Interfaces;
using CTM.Models;
using CTMLocalizationLib.Resources;

namespace CTM.Areas.ManageData.ViewModels.EnglishTests
{
    public class UploadTemplate :UploadTemplateBase, IUploadTemplate, IEnglishTest
    {
        [Display(Name = "CabinAnnoucement", ResourceType = typeof(ConstModels))]
        public string CabinAnnoucement { get; set; }

        [Display(Name = "SpokenSkill", ResourceType = typeof(ConstModels))]
        public string SpokenSkill { get; set; }

    }
}