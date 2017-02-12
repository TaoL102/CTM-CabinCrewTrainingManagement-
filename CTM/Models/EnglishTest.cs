using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CTM.Codes.Helpers;

namespace CTM.Models
{
    public class EnglishTest : Model<EnglishTest>
    {
        private DateTime date;
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public string ID { get; set; }

        [Required]
        [Display(Name = "CabinCrewID", ResourceType = typeof(Resources.Models.ConstModels))]
        public string CabinCrewID { get; set; }

        [Required]
        [EnumDataType(typeof(EnglishTestType))]
        [Display(Name = "SubCategory", ResourceType = typeof(Resources.Models.ConstModels))]
        public EnglishTestType Type { get; set; }

        [Required]
        [Display(Name = "Grade", ResourceType = typeof(Resources.Models.ConstModels))]
        public string Grade { get; set; }

        [Required]
        [Display(Name = "CategoryID", ResourceType = typeof(Resources.Models.ConstModels))]
        public string CategoryID { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = "Date", ResourceType = typeof(Resources.Models.ConstModels))]
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

        [Display(Name = "Category", ResourceType = typeof(Resources.Models.ConstModels))]
        public virtual Category Category { get; set; }

        [Display(Name = "CabinCrew", ResourceType = typeof(Resources.Models.ConstModels))]
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
        [Display(Name = "CabinAnnoucement", ResourceType = typeof(Resources.Models.ConstModels))]
        CabinAnnoucement = 000000000001,

        [Display(Name = "SpokenSkill", ResourceType = typeof(Resources.Models.ConstModels))]
        SpokenSkill = 000000000002,

    };


}
