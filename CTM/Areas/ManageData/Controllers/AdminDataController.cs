using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using CTM.Codes.Database;
using CTM.Controllers;
using CTMLib.Models;

namespace CTM.Areas.ManageData.Controllers
{
    public class AdminDataController : BaseController
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