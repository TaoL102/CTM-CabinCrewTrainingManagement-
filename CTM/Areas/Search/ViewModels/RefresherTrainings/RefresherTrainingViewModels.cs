using System;
using System.ComponentModel.DataAnnotations;

namespace CTM.Areas.Search.ViewModels.RefresherTrainings
{
        public class DisplayRefresherTrainingsViewModel
        {

            public string ID { get; set; }

            [Display(Name = "姓名")]
            public string CabinCrewID { get; set; }

            [Display(Name = "姓名")]
            public string CabinCrewName { get; set; }

            [Display(Name = "类型")]
            public string CategoryName { get; set; }

            [Display(Name = "备注")]
            public string Remark { get; set; }

            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            [DataType(DataType.Date)]
            [Display(Name = "日期")]
            public DateTime Date { get; set; }


            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            [DataType(DataType.Date)]
            [Display(Name = "失效日")]
            public DateTime ExpiryDate
            {
                get
                {
                    DateTime lastDate = new DateTime(Date.AddMonths(13).Year, Date.AddMonths(13).Month, 1).AddMonths(1).AddDays(-1);
                    return lastDate;
                }

            }

        }
    }
