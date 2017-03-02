using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CTM.Areas.ManageData.ViewModels.EnglishTests;
using Microsoft.AspNet.Identity;
using CTM.Codes.Database;
using CTM.Controllers;
using CTMLib.Helpers;
using CTMLib.Models;

namespace CTM.Areas.ManageData.Controllers
{
    public class EnglishTestsController : BaseController
    {
        private readonly DbManager _dbManager=new DbManager();

        // GET: EnglishTests/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(_dbManager.DbSet<Category>().Where(o => o.Type == SuperCategory.英语考核), "ID", "Name");
            return PartialView("_CreatePartial");
        }

        // POST: EnglishTests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,ID,Type,Grade,CategoryID,Date")] EnglishTest englishTest, string CCName)
        {
            ModelState.Remove("ID");
            ModelState.Remove("ID");

            if (ModelState.IsValid)
            {
                // Cabin Crew
                var cabinCrew =await  _dbManager.DbSet<CabinCrew>().FirstOrDefaultAsync(o => o.Name.ToLower().Equals(CCName.Trim().ToLower()));
                var category = await _dbManager.Categories.FirstOrDefaultAsync(o => o.ID.Equals(englishTest.CategoryID));

                if (cabinCrew != null)
                {
                    englishTest.CabinCrewID = cabinCrew.ID;
                    englishTest.CabinCrew = cabinCrew;
                    englishTest.Category = category;

                    await _dbManager.Add(englishTest);
                    await _dbManager.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

            }

            var cabinCrewList = _dbManager.DbSet<CabinCrew>().OrderBy(o => o.Name).ToList();
            ViewBag.CabinCrewID = new SelectList(cabinCrewList, "ID", "Name");
            ViewBag.CategoryID = new SelectList(_dbManager.DbSet<Category>().Where(o => o.Type == SuperCategory.英语考核), "ID", "Name", englishTest.CategoryID);
            return View(englishTest);
        }

        // GET: EnglishTests/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                 return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnglishTest englishTest = await _dbManager.GetEntityAsync<EnglishTest>(id);
            if (englishTest == null)
            {
                 return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.CategoryID = new SelectList(_dbManager.DbSet<Category>().Where(o => o.Type == SuperCategory.英语考核), "ID", "Name", englishTest.CategoryID);

            return PartialView("_EditPartial", englishTest);
        }

        // POST: EnglishTests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,CabinCrewID,Type,Grade,CategoryID,Date")] EnglishTest englishTest)
        {
            if (ModelState.IsValid)
            {
                await _dbManager.Update(englishTest);
                await _dbManager.SaveChangesAsync();
                return new HttpStatusCodeResult(HttpStatusCode.Accepted);
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }

        // GET: EnglishTests/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                // return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnglishTest englishTest = await _dbManager.GetEntityAsync<EnglishTest>(id);
            if (englishTest == null)
            {
                // return HttpNotFound();
            }

            return PartialView("_DeletePartial", englishTest);
        }

        // POST: EnglishTests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            try
            {
                await _dbManager.Remove<EnglishTest>(id);
                await _dbManager.SaveChangesAsync();
                return new HttpStatusCodeResult(HttpStatusCode.Accepted);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // GET: EnglishTests/Upload
        public ActionResult Upload()
        {
            Upload uploadModelView = new Upload()
            {
                CategoryList =
                    new SelectList(_dbManager.Categories.Where(o => o.Type == SuperCategory.英语考核), "ID", "Name")
            };

            return PartialView("_UploadPartial", uploadModelView);
        }

        // POST: EnglishTests/Upload
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Upload(Upload uploadModelView)
        {
            // EXCEL upload
            HttpPostedFileBase fileBase = Request.Files.Get(0);
            if (fileBase != null && fileBase.ContentLength > 0)
            {
                // Check file type
                if (ExcelHelper.CheckIsExcel(fileBase))
                {

                    // Save to TABLE UploadRecord
                    await _dbManager.Add<UploadRecord>(new UploadRecord()
                    {
                        ID = uploadModelView.UploadRecordID,
                        CategoryID = uploadModelView.CategoryID,
                        DateTime = DateTime.UtcNow,
                        ApplicationUserID = User.Identity.GetUserId(),
                        IsWithdrawn = false,
                    });

                    var englishTestsUpload = ExcelHelper.GenerateListEnglishTestFromExcel(fileBase.InputStream, uploadModelView.Date, uploadModelView.CategoryID, uploadModelView.UploadRecordID);

                    if (englishTestsUpload.Any<EnglishTest>())
                    {
                        _dbManager.AddRange(englishTestsUpload);
                    }

                    var result = await _dbManager.GetContext().SaveChangesAsync().ConfigureAwait(continueOnCapturedContext: false);

                    return new HttpStatusCodeResult(HttpStatusCode.Accepted);
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbManager.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
