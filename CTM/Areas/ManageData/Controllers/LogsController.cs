using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using CTM.Codes.Database;

namespace CTM.Areas.ManageData.Controllers
{
    public class LogsController : Controller
    {
        private CTMDbContext db = new CTMDbContext();

        // GET: ManageData/Logs
        public async Task<ActionResult> Index()
        {
            return View(await db.Logs.ToListAsync());
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
