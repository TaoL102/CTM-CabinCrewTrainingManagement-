using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CTM.Models
{
    public class RefresherTraining : Model<RefresherTraining>
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public string ID { get; set; }

        [Required]
        public string CabinCrewID { get; set; }

        [Required]
        public string CategoryID { get; set; }

        [Display(Name = "Remark", ResourceType = typeof(Resources.Models.ConstModels))]
        public string Remark { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = "Date", ResourceType = typeof(Resources.Models.ConstModels))]
        public DateTime Date { get; set; }

        public string UploadRecordID { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = "ExpiryDate", ResourceType = typeof(Resources.Models.ConstModels))]
        public DateTime ExpiryDate
        {
            get
            {
                DateTime lastDate = new DateTime(Date.AddMonths(13).Year, Date.AddMonths(13).Month, 1).AddMonths(1).AddDays(-1);
                return lastDate;
            }

        }


        public virtual Category Category { get; set; }
        public virtual CabinCrew CabinCrew { get; set; }
        public virtual UploadRecord UploadRecord { get; set; }

        public override string ToString()
        {
            List<string> stringList = new List<string>()
            {
                PropertyToString("ID",ID),
                PropertyToString("CabinCrew",CabinCrew.Name),
                PropertyToString("Category",Category.Name),
                PropertyToString("Date",Date),
                PropertyToString("ExpiryDate",ExpiryDate),
                PropertyToString("Remark",Remark),

            };

            return string.Join(";", stringList);
        }
    }
}