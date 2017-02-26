using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CTM.Areas.ManageAccount.Models;
using CTM.Codes.Database;
using CTM.Controllers;
using CTMLib.Helpers;
using CTMLib.Models;
using Newtonsoft.Json;

namespace CTM.Areas.ManageData.Controllers
{
    public class CabinCrewsController : BaseController
    {
        private CTMDbContext db = new CTMDbContext();

        // GET: CabinCrews
        public async Task<ActionResult> Index()
        {
            var list = await db.CabinCrews.ToListAsync();
            CultureInfo culture = new CultureInfo("zh-CN");
            return View(list.OrderBy(o => o.Name, StringComparer.Create(culture, false)));
        }

        // GET: CabinCrews/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CabinCrew cabinCrew = await db.CabinCrews.FindAsync(id);
            if (cabinCrew == null)
            {
                return HttpNotFound();
            }
            return View(cabinCrew);
        }

        // GET: CabinCrews/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CabinCrews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Name,Gender,Age,EmploymentStartDate,ContractType,AcademicDegree,EducationInstitute,Major,CertificateType,CertificateIssuer,IsResigned")] CabinCrew cabinCrew)
        {
            if (ModelState.IsValid)
            {
                db.CabinCrews.Add(cabinCrew);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(cabinCrew);
        }

        // GET: CabinCrews/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CabinCrew cabinCrew = await db.CabinCrews.FindAsync(id);
            if (cabinCrew == null)
            {
                return HttpNotFound();
            }
            return View(cabinCrew);
        }

        // POST: CabinCrews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Name,Gender,Age,EmploymentStartDate,ContractType,AcademicDegree,EducationInstitute,Major,CertificateType,CertificateIssuer,IsResigned")] CabinCrew cabinCrew)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cabinCrew).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(cabinCrew);
        }

        // GET: CabinCrews/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CabinCrew cabinCrew = await db.CabinCrews.FindAsync(id);
            if (cabinCrew == null)
            {
                return HttpNotFound();
            }
            return View(cabinCrew);
        }

        // POST: CabinCrews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            CabinCrew cabinCrew = await db.CabinCrews.FindAsync(id);
            db.CabinCrews.Remove(cabinCrew);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: CabinCrews/Upload
        public ActionResult Upload()
        {
            return View();
        }

        // POST: CabinCrews/Upload
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Upload(HttpPostedFileBase upload)
        {
            // EXCEL upload
            if (upload != null && upload.ContentLength > 0)
            {
                // Check file type
                if (ExcelHelper.CheckIsExcel(upload))
                {

                    var cabinCrewsInUpload = ExcelHelper.GenerateListCabinCrewFromExcel(upload.InputStream);

                    // Check if cabin crew exists in current database
                    // If true, update it
                    // If false, add it
                    // if some records in current database donot exist in upload file, set IsResigned true
                    var cabinCrewsInDb = db.CabinCrews.ToList();

                    // If intersected, update
                    var listIntersection = cabinCrewsInDb.Intersect(cabinCrewsInUpload, new CabinCrewComparer());
                    if (listIntersection.Any())
                    {
                        foreach (CabinCrew c in listIntersection)
                        {

                            var ccInDb = await db.CabinCrews.FindAsync(c.ID);
                            var ccInUpload = cabinCrewsInUpload.Where(o => o.ID.Equals(c.ID)).FirstOrDefault();
                            if (!ccInDb.Equals(ccInUpload))
                            {
                                ccInDb.Name = ccInUpload.Name;
                                ccInDb.IsResigned = false;
                            }
                        }
                    }


                    // If not existed in db, but in listUploaded
                    var listDifference1 = cabinCrewsInUpload.Except(cabinCrewsInDb, new CabinCrewComparer());
                    if (listDifference1.Any())
                    {
                        db.CabinCrews.AddRange(listDifference1);
                    }

                    // If not existed in listUpload, but in db, 
                    var listDifference2 = cabinCrewsInDb.Except(cabinCrewsInUpload, new CabinCrewComparer());
                    if (listDifference2.Any())
                    {
                        foreach (CabinCrew c in listDifference2)
                        {
                            c.IsResigned = true;
                        }
                    }
                    await db.SaveChangesAsync().ConfigureAwait(continueOnCapturedContext: false);
                    return RedirectToAction("Index");
                }
            }
            return View();
        }


        public  JsonResult GetCabinCrewNames(string name)
        {

            CultureInfo culture = new CultureInfo("zh-CN");
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

    public class CabinCrewComparer : IEqualityComparer<CabinCrew>
    {
        public bool Equals(CabinCrew x, CabinCrew y)
        {
            return x.ID == y.ID;
        }

        public int GetHashCode(CabinCrew obj)
        {
            return obj.ID.GetHashCode();
        }
    }
}
