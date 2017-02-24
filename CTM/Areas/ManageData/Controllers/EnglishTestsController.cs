using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CTM.Areas.ManageAccount.Models;
using EntityFramework.BulkInsert.Extensions;
using Microsoft.AspNet.Identity;
using PagedList;
using CTM;
using CTM.Codes.Database;
using CTM.Controllers;
using CTMLib.Helpers;
using CTMLib.Models;
using static System.String;

namespace CTM.Areas.ManageData.Controllers
{
    public class EnglishTestsController : BaseController
    {
        private readonly DbManager _dbManager=new DbManager();

        // Parameters
        List<object> parameterValues ;
        List<string> parameterNames ;

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
        public async Task<PartialViewResult> Edit(string id)
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

            ViewBag.CategoryID = new SelectList(_dbManager.DbSet<Category>().Where(o => o.Type == SuperCategory.英语考核), "ID", "Name", englishTest.CategoryID);

            return PartialView("_EditPartial", englishTest);
        }

        // POST: EnglishTests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,ID,Type,Grade,CategoryID,Date")] EnglishTest englishTest)
        {
            if (ModelState.IsValid)
            {
                await _dbManager.Update(englishTest);
                await _dbManager.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(_dbManager.Categories.Where(o => o.Type == SuperCategory.英语考核), "ID", "Name", englishTest.CategoryID);

            return View(englishTest);
        }

        // GET: EnglishTests/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                await _dbManager.Remove<EnglishTest>(id);
                await _dbManager.SaveChangesAsync();

                return new HttpStatusCodeResult(HttpStatusCode.Accepted);
            }
        }


        // GET: EnglishTests/Upload
        public ActionResult Upload()
        {
            // Dropdownlist for EnglishTestCategory
            ViewBag.EnglishTestCategoryList = new SelectList(_dbManager.Categories.Where(o => o.Type == SuperCategory.英语考核), "ID", "Name");

            return PartialView("_UploadPartial");
        }

        // POST: EnglishTests/Upload
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

                    // Get category name


                    // Save to TABLE UploadRecord
                    await _dbManager.Add<UploadRecord>(new UploadRecord()
                    {
                        ID = uploadRecordID,
                        CategoryID = categoryID,
                        DateTime = DateTime.UtcNow,
                        ApplicationUserID = User.Identity.GetUserId(),
                        IsWithdrawn = false,

                    });

                    var englishTestsUpload = ExcelHelper.GenerateListEnglishTestFromExcel(upload.InputStream, date, categoryID, uploadRecordID);

                    if (englishTestsUpload.Any<EnglishTest>())
                    {
                        _dbManager.AddRange(englishTestsUpload);
                    }

                    var result = await _dbManager.GetContext().SaveChangesAsync().ConfigureAwait(continueOnCapturedContext: false);

                    return RedirectToAction("Index");
                }
            }
            return View("Index");
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
