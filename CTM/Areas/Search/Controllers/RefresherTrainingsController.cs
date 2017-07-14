using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using CTM.Areas.Search.ViewModels.RefresherTrainings;
using CTM.Codes.Database;
using CTM.Models;
using CTM.Codes.Helpers;

namespace CTM.Areas.Search.Controllers
{
    public class RefresherTrainingsController : Controller
    {
        private CTMDbContext db = new CTMDbContext();
        private static CultureInfo culture = new CultureInfo("zh-CN");

        public async Task<ActionResult> Index()
        {
            // Dropdownlist for CategoryName
            ViewBag.CategoryList = new SelectList(db.Categories.Where(o => o.Type == SuperCategory.RefresherTraining), "ID", "Name");

            return View();
        }

        // GET: RefresherTrainings
        public async Task<ActionResult> Search(string CCName, string CategoryID, string Date, string UploadRecordID, bool IsLatest = false)
        {
            // If all parameters are null, return view without any data
            if (CCName == null && CategoryID == null && Date == null)
            {
                ViewBag.IsLatest = false;
                return null;
            }

            // Get list
            var list = GetResultByFilter(CCName, CategoryID, Date, UploadRecordID, IsLatest);

            // If not ajax call , return file for downloading
            if (Request.IsAjaxRequest())
            {
                return PartialView("_SearchResultPartial", list);
            }
            return ExportToFile("RefresherTraining", IsLatest, list);


        }


        // 
        public List<DisplayRefresherTrainingsViewModel> GetResultByFilter(string CCName, string CategoryID, string Date, string UploadRecordID, bool IsLatest = false)
        {
            // Parameters
            List<object> parameterValues = new List<object>();
            List<string> parameterNames = new List<string>();


            if (!String.IsNullOrEmpty(CCName))
            {
                parameterValues.Add(new SqlParameter("Name", CCName));
                parameterNames.Add(ConstantHelper.TableNameCabinCrews + ".[Name]=@Name");
            }

            if (!String.IsNullOrEmpty(CategoryID))
            {
                parameterNames.Add("CategoryID=@CategoryID");
                parameterValues.Add(new SqlParameter("CategoryID", CategoryID));
            }

            if (!String.IsNullOrEmpty(Date))
            {
                try
                {
                    // Convert time
                    DateTime date = DateTime.Parse(Date);
                    parameterNames.Add("Month(Date)=@Month");
                    parameterNames.Add("Year(Date)=@Year");
                    parameterValues.Add(new SqlParameter("Month", date.Month));
                    parameterValues.Add(new SqlParameter("Year", date.Year));
                }
                catch (Exception)
                {

                }

            }


            // Build SQL
            string sqlString = "";
            if (IsLatest)
            {
                ViewBag.IsLatest = true;

                // Build sql
                StringBuilder sb = new StringBuilder();
                if (!String.IsNullOrEmpty(CCName))
                {
                    sb.Append(" WHERE ").Append(ConstantHelper.TableNameCabinCrews + ".[Name]=@Name ");
                }

                sqlString = SqlQueryHelper.GetSqlRefresherTrainingIsLatest(sb.ToString());
            }
            else
            {
                ViewBag.IsLatest = false;

                // Build sql
                StringBuilder sb = new StringBuilder();
                if (parameterNames.Any())
                {
                    sb.Append(" WHERE ");
                    sb.Append(string.Join(" AND ", parameterNames));
                    sb.Append(" ");
                }

                sqlString = SqlQueryHelper.GetSqlRefresherTraining(sb.ToString());
            }

            // Get data from database
            var returnList = db.Database.SqlQuery<DisplayRefresherTrainingsViewModel>(sqlString, parameterValues.ToArray()).ToList();

            // Order by date, then by name
            DateTime now = DateTime.Today;
            DateTime currentMonthDate = new DateTime(now.Year, now.Month, 1).AddMonths(1).AddDays(-1);
            DateTime addedOneMonthDate = new DateTime(now.Year, now.Month, 1).AddMonths(2).AddDays(-1);
            DateTime addedTwoMonthDate = new DateTime(now.Year, now.Month, 1).AddMonths(3).AddDays(-1);
            var list = returnList
                .OrderByDescending(o => o.ExpiryDate.Date == currentMonthDate)
                .ThenByDescending(o => o.ExpiryDate.Date == addedOneMonthDate)
                .ThenByDescending(o => o.ExpiryDate.Date == addedTwoMonthDate)
                .ThenBy(o => o.ExpiryDate)
                .ThenBy(o => o.CabinCrewName, StringComparer.Create(culture, false)).ToList();

            return list;

        }


        //// POST: RefresherTrainings/DownloadTemplate
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //public ActionResult DownloadTemplate()
        //{
        //    // Get 
        //    var listCabinCrews = db.CabinCrews.ToList();

        //    var stream = ExcelHelper.GenerateRefresherTrainingsTemplate(listCabinCrews); // Return a MemoryStream 

        //    stream.Seek(0, SeekOrigin.Begin);

        //    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Refresher Trainings Template.xlsx");
        //}


        private FileStreamResult ExportToFile(string filename, bool isLatest, List<DisplayRefresherTrainingsViewModel> list)
        {
            // header
            List<string> hearderList = isLatest
                ? new List<string>() { "员工ID", "姓名", "类型", "日期", "失效日期", "备注", }
                : new List<string>() { "员工ID", "姓名", "类型", "日期", "备注" };


            // list
            var listSelected = new List<object[]>();

            if (isLatest)
            {
                foreach (var var in list)
                {
                    object[] obj = new object[]
                   {
                 var.CabinCrewID,
                 var.CabinCrewName,
                 var.CategoryName,
                   var.Date.ToShortDateString(),
                   var.ExpiryDate.ToShortDateString(),
                 var.Remark,

                   };
                    listSelected.Add(obj);
                }
            }
            else
            {
                foreach (var var in list)
                {
                    object[] obj = new object[]
                   {
                 var.CabinCrewID,
                 var.CabinCrewName,
                 var.CategoryName,
                       var.Date.ToShortDateString(),
                 var.Remark,

                   };
                    listSelected.Add(obj);
                }
            }

            // Must be deleted
            return null;

            // var stream = ExcelHelper.GenerateExcel(filename, listSelected, hearderList); // Return a MemoryStream 

            //  stream.Seek(0, SeekOrigin.Begin);

            //  return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename + ".xlsx");
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
