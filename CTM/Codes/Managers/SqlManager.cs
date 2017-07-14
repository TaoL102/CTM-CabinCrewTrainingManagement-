using CTM.Codes.Helpers;
using System.Collections.Generic;

namespace CTM.Codes.Managers
{
    public static class SqlManager
    {
        private static readonly Dictionary<string, ISqlAction> SqlActionDic = new Dictionary<string, ISqlAction>()
        {
            {ConstantHelper.ControllerNameEnglishTest,new SqlEnglishTest()},
            {ConstantHelper.ControllerNameRefresherTraining,null },
            {ConstantHelper.ControllerNameCabinCrew,null },
            {ConstantHelper.ControllerNameCategory,null },
            {ConstantHelper.ControllerNameLog,null },
            {ConstantHelper.ControllerNameUploadRecord,null }
        };

        public static ISqlAction GetSqlAction<T>(T type)
        {
            return SqlActionDic[ControllerHelper<T>.GetControllerName()];
        }
    }
}