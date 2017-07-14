using System.Web.Mvc;
using CTM.Codes.Database;
using CTM.Codes.Managers;
using CTM.Controllers;

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


    }
}