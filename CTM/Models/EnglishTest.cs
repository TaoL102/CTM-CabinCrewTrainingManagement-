using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CTM.Codes.Interfaces;

namespace CTM.Models
{
    public class EnglishTest : Model,IEnglishTest
    {
        private DateTime date;
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public string ID { get; set; }

        [Required]
        [Display(Name = "CabinCrewID", ResourceType = typeof(CTMLocalizationLib.Resources.ConstModels))]
        public string CabinCrewID { get; set; }

        [Required]
        [EnumDataType(typeof(EnglishTestType))]
        [Display(Name = "SubCategory", ResourceType = typeof(CTMLocalizationLib.Resources.ConstModels))]
        public EnglishTestType Type { get; set; }

        [Required]
        [Display(Name = "Grade", ResourceType = typeof(CTMLocalizationLib.Resources.ConstModels))]
        public string Grade { get; set; }

        [Required]
        [Display(Name = "Category", ResourceType = typeof(CTMLocalizationLib.Resources.ConstModels))]
        public string CategoryID { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = "Date", ResourceType = typeof(CTMLocalizationLib.Resources.ConstModels))]
        public DateTime Date
        {
            get
            {
                return date.ToLocalTime();
            }
            set
            {
                date = value;
            }
        }

        public string UploadRecordID { get; set; }

        [Display(Name = "Category", ResourceType = typeof(CTMLocalizationLib.Resources.ConstModels))]
        public virtual Category Category { get; set; }

        [Display(Name = "CabinCrew", ResourceType = typeof(CTMLocalizationLib.Resources.ConstModels))]
        public virtual CabinCrew CabinCrew { get; set; }
        public virtual UploadRecord UploadRecord { get; set; }

        public EnglishTest()
        {
            ID=Guid.NewGuid().ToString();
        }

        public override string ToString()
        {
            List<string> stringList = new List<string>()
            {
                PropertyToString("ID",ID),
                PropertyToString("CabinCrew",CabinCrew.Name),
                PropertyToString("Category",Category.Name),
                PropertyToString("Type",Type),
                PropertyToString("Grade",Grade),
                PropertyToString("Date",Date)
            };

            return string.Join(";", stringList);
        }
    }



    public enum EnglishTestType
    {
        [Display(Name = "CabinAnnoucement", ResourceType = typeof(CTMLocalizationLib.Resources.ConstModels))]
        CabinAnnoucement = 000000000001,

        [Display(Name = "SpokenSkill", ResourceType = typeof(CTMLocalizationLib.Resources.ConstModels))]
        SpokenSkill = 000000000002,

    };


}
