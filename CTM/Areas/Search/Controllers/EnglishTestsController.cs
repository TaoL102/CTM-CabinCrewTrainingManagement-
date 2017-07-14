using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CTM.Controllers;
using CTM.Models;
using CTMCustomControlLib.Models;
using CTM.Codes.Helpers;
using CTM.Codes.Interfaces;
using CTM.Codes.Managers;

namespace CTM.Areas.Search.Controllers
{
    public class EnglishTestsController  : ControllerSearchBase
    {       

        /// <summary>
        /// Index Page
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            var categoryList = DbManager.DbSet<Category>().Where(o => o.Type == SuperCategory.EnglishTest);
            var searchViewModel = new ViewModels.EnglishTests.Search
            {
                CategoryList =
                    new SelectList(categoryList, "ID", "Name")
            };
            return View(searchViewModel);
        }

        /// <summary>
        /// Search Result
        /// </summary>
        /// <param name="searchViewModel"></param>
        /// <returns></returns>
        public async Task<ActionResult> Search(ViewModels.EnglishTests.Search searchViewModel)
        {
            return await SearchByViewModel(searchViewModel);
        }

    }
}
