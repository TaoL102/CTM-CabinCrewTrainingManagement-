using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using CTMLib.CustomControls;
using CTMLib.CustomControls.Alert;
using CTMLib.CustomControls.Button;

namespace CTMLib.Helpers
{
    static class CssHelper<T>
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
            if (type.GetInterfaces().Contains(typeof(IButtonControlBase)))
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
