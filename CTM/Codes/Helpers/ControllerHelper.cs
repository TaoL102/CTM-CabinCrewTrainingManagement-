using System.Web.Mvc;
using CTM.Areas.Search.Controllers;
using CTM.Codes.Interfaces;
using CTM.Models;

namespace CTM.Codes.Helpers
{
    public static class ControllerHelper<T> 
    {
        public static string GetControllerName()
        {
            var type = typeof(T);
            if (typeof(IEnglishTest).IsAssignableFrom(type))
            {
                return ConstantHelper.ControllerNameEnglishTest;
            }
            //if (type.IsSubclassOf(typeof(Model<RefresherTraining>)))
            //{
            //    return ConstantHelper.ControllerNameRefresherTraining;
            //}
            //if (type.IsSubclassOf(typeof(Model<CabinCrew>)))
            //{
            //    return ConstantHelper.ControllerNameCabinCrew;
            //}
            //if (type.IsSubclassOf(typeof(Model<Category>)))
            //{
            //    return ConstantHelper.ControllerNameCategory;
            //}
            //if (type.IsSubclassOf(typeof(Model<Log>)))
            //{
            //    return ConstantHelper.ControllerNameLog;
            //}
            //if (type.IsSubclassOf(typeof(Model<GenerateUploadRecord>)))
            //{
            //    return ConstantHelper.ControllerNameUploadRecord;
            //}
            return null;
        }

    }

}