using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CTM.Areas.Search.ViewModels;
using CTM.Controllers;
using CTM.Models;
using CTMCustomControlLib.Models;
using CTM.Codes.Helpers;
using CTM.Codes.Interfaces;
using CTM.Codes.Managers;
using WebGrease.Css.Extensions;

namespace CTM.Areas.Search.Controllers
{
    public class ControllerSearchBase : BaseController,IEnglishTest
    {
        /// <summary>
        /// Get search results by search view model
        /// </summary>
        /// <param name="searchViewModel"></param>
        /// <returns></returns>
        protected async Task<ActionResult> SearchByViewModel(ISearchViewModel searchViewModel)
        {
            IEnumerable<ISearchResultModel> list;
            int totalNum;
            string viewName;
            ISqlAction sqlAction = SqlManager.GetSqlAction(this);

            // Get result list and total result mumber
            if (searchViewModel.IsLatest)
            {
                list = sqlAction.GetResultIsLatestByFilter(searchViewModel);
                totalNum = sqlAction.GetTotalNumberByFilter(searchViewModel);
                viewName = "_SearchResultIsLatestPartial";
            }
            else
            {
                list = sqlAction.GetResultByFilter(searchViewModel);
                totalNum = sqlAction.GetTotalNumberByFilter(searchViewModel);
                viewName = "_SearchResultPartial";
            }

            // If requested download
            if (searchViewModel.IsDownload)
            {
                return ExportToExcel(list.ToList());
            }

            // Otherwise return view
            ViewBag.Pager = new Pager(totalNum, searchViewModel.Page, ConstantHelper.PaginationPageSize);
            ViewBag.SearchResultViewModel = searchViewModel;
            return PartialView(viewName, list);
        }

        protected FileStreamResult ExportToExcel(IList<ISearchResultModel> list)
        {
            string fileName = "Export" + new DateTime().Date;
            var stream = ExcelHelper.GenerateExcel(fileName, list);
              stream.Seek(0, SeekOrigin.Begin);
              return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName + ".xlsx");
        }
    }
}
