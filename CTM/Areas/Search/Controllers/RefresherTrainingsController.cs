using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CTM.Areas.ManageAccount.Models;
using CTM.Areas.Search.Models;
using CTM.Codes.Common;
using CTM.Codes.Helpers;
using CTM.Models;
using Microsoft.AspNet.Identity;

namespace CTM.Areas.Search.Controllers
{
    public class RefresherTrainingsController : Controller
    {
        private CTMDbContext db = new CTMDbContext();
        private static CultureInfo culture = new CultureInfo("zh-CN");

        public async Task<ActionResult> Index()
        {
            // Dropdownlist for CategoryName
            ViewBag.CategoryList = new SelectList(db.Categories.Where(o => o.Type == SuperCategory.复训), "ID", "Name");

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
            return ExportToFile("复训", IsLatest, list);


        }


        // 
        public List<RefresherTrainingViewModels.DisplayRefresherTrainingsViewModel> GetResultByFilter(string CCName, string CategoryID, string Date, string UploadRecordID, bool IsLatest = false)
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
            var returnList = db.Database.SqlQuery<RefresherTrainingViewModels.DisplayRefresherTrainingsViewModel>(sqlString, parameterValues.ToArray()).ToList();

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


        // GET: RefresherTrainings/Create
        public ActionResult Create()
        {
            var cabinCrewList = db.CabinCrews.OrderBy(o => o.Name).ToList();
            ViewBag.CabinCrewID = new SelectList(cabinCrewList, "ID", "Name");
            ViewBag.CategoryID = new SelectList(db.Categories.Where(o => o.Type == SuperCategory.复训), "ID", "Name");
            return View();
        }

        // POST: RefresherTrainings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,CabinCrewID,CategoryID,Remark,Date,UploadRecordID")] RefresherTraining refresherTraining)
        {
            ModelState.Remove("ID");
            if (ModelState.IsValid)
            {
                refresherTraining.ID = Guid.NewGuid().ToString();
                db.RefresherTrainings.Add(refresherTraining);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            var cabinCrewList = db.CabinCrews.OrderBy(o => o.Name).ToList();

            ViewBag.CabinCrewID = new SelectList(cabinCrewList, "ID", "Name", refresherTraining.CabinCrewID);
            ViewBag.CategoryID = new SelectList(db.Categories.Where(o => o.Type == SuperCategory.复训), "ID", "Name", refresherTraining.CategoryID);
            return View(refresherTraining);
        }

        // GET: RefresherTrainings/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RefresherTraining refresherTraining = await db.RefresherTrainings.FindAsync(id);
            if (refresherTraining == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories.Where(o => o.Type == SuperCategory.复训), "ID", "Name", refresherTraining.CategoryID);
            return View(refresherTraining);
        }

        // POST: RefresherTrainings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,CabinCrewID,CategoryID,Remark,Date,UploadRecordID")] RefresherTraining refresherTraining)
        {
            if (ModelState.IsValid)
            {
                db.Entry(refresherTraining).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories.Where(o => o.Type == SuperCategory.复训), "ID", "Name", refresherTraining.CategoryID);
            return View(refresherTraining);
        }

        // GET: RefresherTrainings/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RefresherTraining refresherTraining = await db.RefresherTrainings.FindAsync(id);
            if (refresherTraining == null)
            {
                return HttpNotFound();
            }
            return View(refresherTraining);
        }

        // POST: RefresherTrainings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            RefresherTraining refresherTraining = await db.RefresherTrainings.FindAsync(id);
            db.RefresherTrainings.Remove(refresherTraining);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: RefresherTrainings/Upload
        public ActionResult Upload()
        {
            // Dropdownlist for CategoryName
            var list = db.Categories.Where(o => o.Type == SuperCategory.复训);
            ViewBag.CategoryList = new SelectList(list, "ID", "Name");

            return View();
        }

        // POST: RefresherTrainings/Upload
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Upload(HttpPostedFileBase upload, string categoryID, DateTime date)
        {
            // EXCEL upload
            if (upload != null && upload.ContentLength > 0)
            {
                // Check file type
                if (ExcelHelper.CheckIsExcel(upload))
                {
                    // uploadRecordID
                    var uploadRecordID = Guid.NewGuid().ToString();
                    // Save to TABLE UploadRecord
                    db.UploadRecords.Add(new UploadRecord()
                    {
                        ID = uploadRecordID,
                   //  CategoryID = categoryID,
                        DateTime = DateTime.UtcNow,
                        ApplicationUserID = User.Identity.GetUserId(),
                        IsWithdrawn = false,

                    });

                    var refresherTrainingsUpload = ExcelHelper.GenerateListRefresherTrainingFromExcel(upload.InputStream, date, categoryID, uploadRecordID);

                    if (refresherTrainingsUpload.Count<RefresherTraining>() != 0)
                    {
                        db.RefresherTrainings.AddRange(refresherTrainingsUpload);
                    }

                    var result = await db.SaveChangesAsync().ConfigureAwait(continueOnCapturedContext: false);

                    return RedirectToAction("Index");
                }
            }
            return View();
        }


        // POST: RefresherTrainings/DownloadTemplate
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult DownloadTemplate()
        {
            var stream = ExcelHelper.GenerateRefresherTrainingsTemplate(); // Return a MemoryStream 

            stream.Seek(0, SeekOrigin.Begin);

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Refresher Trainings Template.xlsx");
        }


        private FileStreamResult ExportToFile(string filename, bool isLatest, List<RefresherTrainingViewModels.DisplayRefresherTrainingsViewModel> list)
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


            var stream = ExcelHelper.GenerateExcel(filename, listSelected, hearderList); // Return a MemoryStream 

            stream.Seek(0, SeekOrigin.Begin);

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename + ".xlsx");
        }

        private void AddSqlParameter(string pName, object value)
        {


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
