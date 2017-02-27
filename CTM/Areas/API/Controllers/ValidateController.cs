using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CTM.Codes.Database;
using WebGrease.Css.Extensions;

namespace CTM.Areas.API.Controllers
{
    public class ValidateController : Controller
    {
        private CTMDbContext db = new CTMDbContext();
        CultureInfo culture = new CultureInfo("zh-CN");

        public  bool IsValidCabinCrew(string names)
        {
            if (String.IsNullOrEmpty(names))
            return true;

            List<string> errorNamesList=new List<string>();
            // Split
            char[] chars = new char[] {',','，',';'};
            string[] namesArray = names.Replace(" ","").Split(chars, StringSplitOptions.RemoveEmptyEntries);
            namesArray.ForEach( name =>
            {
                bool isExist= db.CabinCrews.Any(o => o.Name == name);
                if (!isExist)
                {
                    errorNamesList.Add(name);
                }
            });

            return !errorNamesList.Any();
        }
    }
}