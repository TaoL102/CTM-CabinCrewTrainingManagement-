using System;
using CTM.Codes.Helpers;

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