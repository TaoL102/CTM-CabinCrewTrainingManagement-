using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using CTM.Areas.Search.ViewModels.EnglishTests;
using CTM.Codes.CustomControls;
using CTM.Codes.Database;
using CTMLib.CustomControls.Table;
using CTMLib.Models;

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

        public async Task<JsonResult> GetSearchResultRow(string controllerName, string id)
        {
            var entity=await new DbManager().GetEntityAsync<EnglishTest>(id);
            SearchResult searchResult=new SearchResult()
            {
                ID = entity.ID,
                CabinCrewID = entity.CabinCrewID,
                CabinCrewName = entity.CabinCrew.Name,
                CategoryName = entity.Category.Name,
                Date = entity.Date,
                Grade = entity.Grade,
                Type = entity.Type
            };
            var result=CustomControlExtension.GetCustomControl(controllerName).Table_SearchResult_Row(searchResult);
            return Json(result, JsonRequestBehavior.AllowGet);
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