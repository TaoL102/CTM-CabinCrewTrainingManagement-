using System;
using System.Linq;
using CTMCustomControlLib.CustomControls;
using CTMCustomControlLib.CustomControls.Button;

namespace CTMCustomControlLib.Helpers
{
    internal static class CssHelper<T>
    {
        public static string ControlTypeAbbr { get; }

        static CssHelper()
        {
            ControlTypeAbbr = GetAbbrOfControlType();
        }

        private static string GetAbbrOfControlType()
        {
            string abbr=null;
            Type type = typeof(T);
            if (type.GetInterfaces().Contains(typeof(IButtonControl)))
            {
                abbr = "btn";
            }
            else
            {
                abbr =type.Name.ToLower().Replace("control","");
            }
            return abbr;
        }

        public static string ConvertToCss(SizeOptions sizeOption)
        {
            string sizeStr = null;
            switch (sizeOption)
            {
                case SizeOptions.Large:
                    sizeStr = "lg";
                    break;
                case SizeOptions.Small:
                    sizeStr = "sm";
                    break;
                case SizeOptions.ExtraSmall:
                    sizeStr = "xs";
                    break;
                default:
                    sizeStr=sizeOption.ToString().ToLower();
                    break;
            }

            return ControlTypeAbbr+"-"+ sizeStr;
        }

        public static string ConvertToCss(ColorOptions colorOption)
        {
            string colorStr=  colorOption.ToString().ToLower();
            return ControlTypeAbbr + "-" + colorStr;
        }
    }
}
