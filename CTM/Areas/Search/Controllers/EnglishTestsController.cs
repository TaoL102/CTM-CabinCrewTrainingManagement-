using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using CTM.Areas.Search.ViewModels.EnglishTests;
using CTM.Codes.Database;
using CTM.Controllers;
using CTMLib.Helpers;
using CTMLib.Models;
using static System.String;

namespace CTM.Areas.Search.Controllers
{
    public class EnglishTestsController : BaseController
    {
        private readonly DbManager _dbManager=new DbManager();

        // Parameters
        List<object> parameterValues ;
        List<string> parameterNames ;

        public async Task<ActionResult> Index()
        {
            // Dropdownlist for EnglishTestCategory
            ViewBag.CategoryID = new SelectList(_dbManager.DbSet<Category>().Where(o => o.Type == SuperCategory.英语考核), "ID", "Name");
            ViewModels.EnglishTests.Search searchViewModel=new ViewModels.EnglishTests.Search();
            searchViewModel.CateforyList= new SelectList(_dbManager.DbSet<Category>().Where(o => o.Type == SuperCategory.英语考核), "ID", "Name");
            return View(searchViewModel);
        }

        // GET: EnglishTests
        public async Task<ActionResult> Search(ViewModels.EnglishTests.Search searchViewModel)
        {
            // Save to local file
            var path = AppDomain.CurrentDomain.BaseDirectory + @"\Log\SQLtrace.txt";
            _dbManager.GetContext().Database.Log = message => System.IO.File.AppendAllText(path, message);

            // Get list
            object list ;
            int totalPage;
            if (searchViewModel.IsLatest)
            {
                list = GetResultIsLatestByFilter(searchViewModel);
                totalPage = GetTotalNumberByFilter(searchViewModel);

            }
            else
            {

                    list = GetResultByFilter(searchViewModel);
                    totalPage = GetTotalNumberByFilter(searchViewModel);

            }


            if (!IsNullOrEmpty(searchViewModel.UploadRecordID))
            {
                return PartialView("_SearchResultPartial", list);
            }

            if (Request.IsAjaxRequest())
            {

                // Return some parameters for pagination 
                ViewBag.Page = searchViewModel.Page ??1;
                ViewBag.CCName = searchViewModel.CCName;
                ViewBag.CategoryID = searchViewModel.CategoryID;
                ViewBag.FromDate = searchViewModel.FromDate;
                ViewBag.ToDate = searchViewModel.ToDate;
                ViewBag.IsLatest = searchViewModel.IsLatest;
                ViewBag.TotalPage = totalPage;

                if (searchViewModel.IsLatest)
                {
                    return PartialView("_SearchResultIsLatestPartial", list);
                }
                return PartialView("_SearchResultPartial", list);
            }
           // return ExportToFile("英语", IsLatest, list);
            return null;
        }


        private string GenerateSqlStringByFilter(ViewModels.EnglishTests.Search searchViewModel, bool IsToalNumber = false)
        {
             parameterValues = new List<object>();
             parameterNames = new List<string>();
            string nameParamsStr = null;

            // Name filter
            if (!IsNullOrEmpty(searchViewModel.CCName))
            {
                // Split
                char[] chars = new char[] { ',', '，', ';' };
                string[] namesArray = searchViewModel.CCName.Replace(" ", "").Split(chars, StringSplitOptions.RemoveEmptyEntries);
                string[] namesArrayParam=new string[namesArray.Length];
                int i = 1;
                foreach (string name in namesArray)
                {
                    // IN clause
                    namesArrayParam[i-1]="@Name" + i ;

                    // parameter
                    parameterValues.Add(new SqlParameter("Name"+i, name));

                    i++;
                }
                nameParamsStr = String.Join(",", namesArrayParam);
                parameterNames.Add(ConstantHelper.TableNameCabinCrews + $".[Name] IN ({nameParamsStr})");
            }

            // Category filter
            if (!IsNullOrEmpty(searchViewModel.CategoryID))
            {
                parameterNames.Add("CategoryID=@CategoryID");
                parameterValues.Add(new SqlParameter("CategoryID", searchViewModel.CategoryID));
            }

            // Date filter
            if (searchViewModel.FromDate!=null && searchViewModel.ToDate!=null)
            {
                try
                {
                    // Convert time
                    parameterNames.Add("Date BETWEEN @FromDate AND @ToDate");
                    parameterValues.Add(new SqlParameter("FromDate", DateTime.Parse(searchViewModel.FromDate.ToString()).ToShortDateString()));
                    parameterValues.Add(new SqlParameter("ToDate", DateTime.Parse(searchViewModel.ToDate.ToString()).ToShortDateString()));
                }
                catch (Exception)
                {

                }

            }

            // Pagination
            int pageSize = ConstantHelper.PaginationPageSize;
            int fromRowNum = ((searchViewModel.Page ?? 1) - 1) * pageSize;
            parameterValues.Add(new SqlParameter("FromRowNum", fromRowNum));
            parameterValues.Add(new SqlParameter("PageSize", pageSize));
            string paginationClause = " OFFSET @FromRowNum ROWS FETCH NEXT @PageSize ROWS ONLY ";


            // Order By
            string orderByClause = @" ORDER BY[TableCabinCrews].[Name] COLLATE Chinese_PRC_CI_AS,[Date] DESC ,  [TableEnglishTests].[Type] ";


            // Uploader ID
            string sqlString = "";
            string sqlStringTotal = "";

            // Build SQL

            if (searchViewModel.IsLatest)
            {
                ViewBag.IsLatest = true;
                // Build sql
                StringBuilder sb = new StringBuilder();
                if (!IsNullOrEmpty(searchViewModel.CCName))
                {
                    sb.Append(" AND ").Append(ConstantHelper.TableNameCabinCrews + $".[Name] IN ({nameParamsStr}) ");
                }

                sqlString = SqlQueryHelper.GetSqlEnglishTestIsLatest(sb.ToString(), paginationClause);
                sqlStringTotal = SqlQueryHelper.GetSqlEnglishTestIsLatestTotal(sb.ToString());
            }
            else
            {
                ViewBag.IsLatest = false;
                // Build sql
                StringBuilder sb = new StringBuilder();
                if (parameterNames.Any())
                {
                    sb.Append(" WHERE ");
                    sb.Append(Join(" AND ", parameterNames));
                    sb.Append(" ");
                }

                sqlString = SqlQueryHelper.GetSqlEnglishTest(sb.ToString(), paginationClause, orderByClause, null);
                sqlStringTotal = SqlQueryHelper.GetSqlEnglishTestTotal(sb.ToString(), orderByClause);
            }
            if (!IsNullOrEmpty(searchViewModel.UploadRecordID))
            {
                StringBuilder sb = new StringBuilder();
                parameterValues.Add(new SqlParameter("UploadRecordID", searchViewModel.UploadRecordID));
                sb.Append(" WHERE ").Append(ConstantHelper.TableNameEnglishTests + ".[UploadRecordID]=@UploadRecordID ");


                sqlString = SqlQueryHelper.GetSqlEnglishTest(sb.ToString(), paginationClause, orderByClause, null); ;
            }

            if (IsToalNumber)
            {
                return sqlStringTotal;
            }
            return sqlString;
        }



        public int GetTotalNumberByFilter(ViewModels.EnglishTests.Search searchViewModel)
        {
            var sqlString = GenerateSqlStringByFilter(searchViewModel, true);

            // get total number 
            try
            {

                    var totalNum = _dbManager.SqlQuery<int>(sqlString, parameterValues.ToArray()).First();
                    var totalPage = (totalNum+ ConstantHelper.PaginationPageSize-1) / ConstantHelper.PaginationPageSize;

                    return totalPage;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }


        public List<SearchResult> GetResultByFilter(ViewModels.EnglishTests.Search searchViewModel)
        {
           //  db.Database.Log = message => System.IO.File.AppendAllText(@"d:\SQLtrace.txt", message);
            var sqlString = GenerateSqlStringByFilter(searchViewModel,
                false);

            // get data from database
                var list = _dbManager.SqlQuery<SearchResult>(sqlString, parameterValues.ToArray()).ToList();
                return list;


        }

        public List<SearchResultIsLatest> GetResultIsLatestByFilter(ViewModels.EnglishTests.Search searchViewModel)
        {
            var sqlString = GenerateSqlStringByFilter(searchViewModel,
                false);

            // get data from database

            var list = _dbManager.SqlQuery<SearchResultIsLatest>(sqlString, parameterValues.ToArray()).ToList();

            return list;

        }

        // POST: EnglishTests/DownloadTemplate
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult DownloadTemplate()
        {
            // Get 
            var listCabinCrews = _dbManager.CabinCrews.ToList();
            var stream = ExcelHelper.GenerateEnglishTestTemplate(listCabinCrews); // Return a MemoryStream 

            stream.Seek(0, SeekOrigin.Begin);

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "English Test Template.xlsx");
        }

        private ActionResult ExportToFile(string filename, bool isLatest, List<SearchResult> list)
        {
            // header
            List<string> hearderList = isLatest
                ? new List<string>() { "员工ID", "姓名", "成绩(CabinAnnoucement)", "类型(CabinAnnoucement)", "日期(CabinAnnoucement)", "成绩(SpokenSkill)", "类型(SpokenSkill)", "日期(SpokenSkill)" }
                : new List<string>() { "员工ID", "姓名", "考核项目", "成绩", "类型", "日期" };

            // list
            var listSelected = new List<object[]>();

            if (isLatest)
            {
                foreach (var var in list.GroupBy(o => o.CabinCrewName))
                {
                    var testAnn = var.FirstOrDefault(o => o.Type == EnglishTestType.CabinAnnoucement) ?? new SearchResult();
                    var testOrl = var.FirstOrDefault(o => o.Type == EnglishTestType.SpokenSkill) ?? new SearchResult();

                    object[] obj = new object[]
                  {
                 testAnn.CabinCrewID,
                 testAnn.CabinCrewName,
                 testAnn.Grade,
                 testAnn.CategoryName,
                 testAnn.Date.ToString(),
                 testOrl.Grade,
                 testOrl.CategoryName,
                 testOrl.Date.ToString()
                  };
                    listSelected.Add(obj);
                }



            }
            else
            {
                foreach (var var in list)
                {
                    object[] obj = new object[]
                   {
                 var.CabinCrewID,
                 var.CabinCrewName,
                 var.Type,
                 var.Grade,
                 var.CategoryName,
                 var.Date.ToString()
                   };
                    listSelected.Add(obj);

                }
            }
            var stream = ExcelHelper.GenerateExcel(filename, listSelected, hearderList); // Return a MemoryStream 

            stream.Seek(0, SeekOrigin.Begin);

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename + ".xlsx");
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
