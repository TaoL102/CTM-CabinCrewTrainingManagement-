using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CTM.Codes.CustomControls;
using CTM.Codes.CustomControls.EnglishTests;
using CTMLib.Models;

namespace CTM.Codes.Helpers
{
    public static class ControllerHelper<T>
    {
        public static string GetControllerName()
        {
            var type = typeof(T);
            if (type.IsSubclassOf(typeof(Model<EnglishTest>)))
            {
                return ConstantHelper.ControllerNameEnglishTest;
            }
            else if (type.IsSubclassOf(typeof(Model<RefresherTraining>)))
            {
                return ConstantHelper.ControllerNameRefresherTraining;
            }
            else if (type.IsSubclassOf(typeof(Model<CabinCrew>)))
            {
                return ConstantHelper.ControllerNameCabinCrew;
            }
            else if (type.IsSubclassOf(typeof(Model<Category>)))
            {
                return ConstantHelper.ControllerNameCategory;
            }
            else if (type.IsSubclassOf(typeof(Model<Log>)))
            {
                return ConstantHelper.ControllerNameLog;
            }
            else if (type.IsSubclassOf(typeof(Model<UploadRecord>)))
            {
                return ConstantHelper.ControllerNameUploadRecord;
            }
            return null;
        }

    }
}