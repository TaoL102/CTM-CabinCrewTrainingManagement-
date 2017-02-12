using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CTM.Areas.Search.Models;

namespace CTM.Models
{
    public class CabinCrew : Model<CabinCrew>, IEquatable<CabinCrew>
    {
        public CabinCrew()
        {
            this.EnglishTests = new HashSet<EnglishTest>();
        }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public string ID { get; set; }

        [Index(IsUnique=true)]
        [StringLength(100)]
        [Display(Name = "CabinCrewName", ResourceType = typeof(Resources.Models.ConstModels))]
        public string Name { get; set; }

        [Display(Name = "IsResigned", ResourceType = typeof(Resources.Models.ConstModels))]
        public bool IsResigned { get; set; }

        public virtual ICollection<EnglishTest> EnglishTests { get; set; }


        public override bool Equals(object obj)
        {
            return this.Equals(obj as CabinCrew);
        }

        public bool Equals(CabinCrew other)
        {
            if (other == null)
                return false;

            return
            this.ID == other.ID &&
            this.Name == other.Name &&
            this.IsResigned == other.IsResigned;
        }

        public int GetHashCode(CabinCrew obj)
        {
            return obj.ID.GetHashCode();
        }

        public override string ToString()
        {
            List<string> stringList = new List<string>()
            {
                PropertyToString("ID",ID),
                PropertyToString("Name",Name),
                PropertyToString("IsResigned",IsResigned)
            };

            return string.Join(";", stringList);
        }
    }
}