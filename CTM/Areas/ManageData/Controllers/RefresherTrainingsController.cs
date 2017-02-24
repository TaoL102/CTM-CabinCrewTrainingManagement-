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
using CTM.Codes.Database;
using CTMLib.Helpers;
using CTMLib.Models;
using Microsoft.AspNet.Identity;

namespace CTM.Areas.ManageData.Controllers
{
    public class RefresherTrainingsController : Controller
    {
        private CTMDbContext db = new CTMDbContext();
        private static CultureInfo culture = new CultureInfo("zh-CN");

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
        public async Task<ActionResult> Create([Bind(Include = "ID,ID,CategoryID,Remark,Date,UploadRecordID")] RefresherTraining refresherTraining)
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
        public async Task<ActionResult> Edit([Bind(Include = "ID,ID,CategoryID,Remark,Date,UploadRecordID")] RefresherTraining refresherTraining)
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
