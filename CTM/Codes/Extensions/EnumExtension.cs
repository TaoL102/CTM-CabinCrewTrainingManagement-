using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using CTM.Codes.Helpers;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace CTM.Codes.Extensions
{
    public static class EnumExtension
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

           return ModelHelper<Enum>.GetPropertyDisplayName(fieldInfo);

        }


    }
}