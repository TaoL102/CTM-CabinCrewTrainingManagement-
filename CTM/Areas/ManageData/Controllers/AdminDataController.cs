using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using CTM.Managers;
using CTMLib.Models;

namespace CTM.Areas.ManageData.Controllers
{
    public class AdminDataController : Controller
    {
        private readonly DbManager _dbManager= new DbManager();

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