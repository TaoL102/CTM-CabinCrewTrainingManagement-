using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CTM.Codes.Database;

namespace CTM.Areas.API.Controllers
{
    public class QueryController : Controller
    {
        private CTMDbContext db = new CTMDbContext();
        private CultureInfo culture = new CultureInfo("zh-CN");

        public JsonResult GetCabinCrewNames(string name)
        {
            var strComparer = StringComparer.Create(culture, true);

            var list =
                db.CabinCrews
                    .Where(o => o.Name.Contains(name) && o.IsResigned.Equals(false))
                    .Select(o => o.Name)
                    .AsEnumerable();

            if (string.IsNullOrEmpty(name))
            {
                return Json(list, JsonRequestBehavior.AllowGet);
            }

            var listArray = list.ToArray();

            Array.Sort(listArray, strComparer);

            return Json(listArray?.Take(10), JsonRequestBehavior.AllowGet);
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}