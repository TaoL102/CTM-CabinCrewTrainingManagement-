using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using CTM.Codes.Managers;
using CTM.Models;

namespace CTM.Areas.ManageData.Controllers
{
    public class AdminDataController : Controller
    {
        private readonly DbManager<Log> _dbManager= new DbManager<Log>();

        // GET: AdminData
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Log()
        {
            return View(await _dbManager.Logs.ToListAsync());
        }
    }
}