using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using CTM.Codes.Database;
using CTM.Models;
using EntityFramework.Extensions;

namespace CTM.Areas.ManageData.Controllers
{
    public class UploadRecordsController : Controller
    {
        private CTMDbContext db = new CTMDbContext();

        // GET: UploadRecords
        public async Task<ActionResult> Index()
        {
            // db.Database.Log = message => System.IO.File.AppendAllText(@"d:\SQLtraceUpload.txt", message);
            var uploadRecords = db.UploadRecords.Include(u => u.ApplicationUser);
            return View(await uploadRecords.OrderByDescending(o => o.DateTime).ToListAsync());
        }

        // GET: UploadRecords/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UploadRecord uploadRecord = await db.UploadRecords.FindAsync(id);
            if (uploadRecord == null)
            {
                return HttpNotFound();
            }
            return View(uploadRecord);
        }







        // GET: UploadRecords/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UploadRecord uploadRecord = await db.UploadRecords.FindAsync(id);
            if (uploadRecord == null)
            {
                return HttpNotFound();
            }
            return View(uploadRecord);
        }

        // POST: UploadRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            UploadRecord uploadRecord = await db.UploadRecords.FindAsync(id);
            db.UploadRecords.Remove(uploadRecord);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        // POST: UploadRecords/Delete/5
        public async Task<ActionResult> WithDraw(string id)
        {
            UploadRecord uploadRecord = await db.UploadRecords.FindAsync(id);

            // Find relevant data

            switch (uploadRecord.Category.Type)
            {
                case SuperCategory.EnglishTest:
                    db.EnglishTests.Where(o => o.UploadRecordID.Equals(id)).Delete();
                    break;
                case SuperCategory.RefresherTraining:
                    db.RefresherTrainings.Where(o => o.UploadRecordID.Equals(id)).Delete();
                    break;
            }

            // Set uploadRecord IsWithdrawn
            uploadRecord.IsWithdrawn = true;
            db.Entry(uploadRecord).State = EntityState.Modified;


            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        // POST: UploadRecords/Delete/5
        public async Task<ActionResult> CheckDetails(string id)
        {
            UploadRecord uploadRecord = await db.UploadRecords.Where(o=>o.ID.Equals(id)).Include(o=>o.Category).FirstOrDefaultAsync();

            // Find relevant data

            switch (uploadRecord.Category.Type)
            {
                case SuperCategory.EnglishTest:

                    return RedirectToAction("Search", "EnglishTests", new { UploadRecordID = id, Area = "Search" });
                    break;
                case SuperCategory.RefresherTraining:
                    return RedirectToAction("Search", "RefresherTrainings", new { UploadRecordID = id });
                    break;
            }

            return RedirectToAction("Index");

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
