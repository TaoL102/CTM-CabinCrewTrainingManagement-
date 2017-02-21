using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CTMLib.Models
{
    public class UploadRecord : Model<UploadRecord>
    {
        private DateTime date;
        public UploadRecord()
        {
            this.EnglishTests = new HashSet<EnglishTest>();
            this.RefresherTraining = new HashSet<RefresherTraining>(); 
        }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public string ID { get; set; }


        public string CategoryID { get; set; }


        public string ApplicationUserID { get; set; }

        [Display(Name = "Description", ResourceType = typeof(Resources.ConstModels))]
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd H:mm:ss}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime)]
        [Display(Name = "DateTime", ResourceType = typeof(Resources.ConstModels))]
        public DateTime DateTime
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

        [Display(Name = "IsWithdrawn", ResourceType = typeof(Resources.ConstModels))]
        public bool IsWithdrawn { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<EnglishTest> EnglishTests { get; set; }
        public virtual ICollection<RefresherTraining> RefresherTraining { get; set; }
    }


}