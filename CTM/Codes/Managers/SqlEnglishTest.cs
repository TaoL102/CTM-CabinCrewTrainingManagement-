using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CTM.Areas.Search.ViewModels;
using CTM.Areas.Search.ViewModels.EnglishTests;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using CTM.Codes.Helpers;

namespace CTM.Codes.Managers
{
    public class SqlEnglishTest: SqlAction
    {
        public override string GenerateSqlStringByFilter(ISearchViewModel model, bool isToalNumber = false)
        {
            ParameterValues = new List<object>();
            ParameterNames = new List<string>();
            string nameParamsStr = null;
            var searchViewModel=(Search)model;

            // Name filter
            if (!String.IsNullOrEmpty(searchViewModel.CCName))
            {
                // Split
                char[] chars = new char[] { ',', '，', ';' };
                string[] namesArray = searchViewModel.CCName.Replace(" ", "").Split(chars, StringSplitOptions.RemoveEmptyEntries);
                string[] namesArrayParam = new string[namesArray.Length];
                int i = 1;
                foreach (string name in namesArray)
                {
                    // IN clause
                    namesArrayParam[i - 1] = "@Name" + i;

                    // parameter
                    ParameterValues.Add(new SqlParameter("Name" + i, name));

                    i++;
                }
                nameParamsStr = String.Join(",", namesArrayParam);
                ParameterNames.Add(ConstantHelper.TableNameCabinCrews + $".[Name] IN ({nameParamsStr})");
            }

            // Category filter
            if (!String.IsNullOrEmpty(searchViewModel.CategoryID))
            {
                ParameterNames.Add("CategoryID=@CategoryID");
                ParameterValues.Add(new SqlParameter("CategoryID", searchViewModel.CategoryID));
            }

            // Date filter
            if (searchViewModel.FromDate != null && searchViewModel.ToDate != null)
            {
                try
                {
                    // Convert time
                    ParameterNames.Add("Date BETWEEN @FromDate AND @ToDate");
                    ParameterValues.Add(new SqlParameter("FromDate", DateTime.Parse(searchViewModel.FromDate.ToString()).ToShortDateString()));
                    ParameterValues.Add(new SqlParameter("ToDate", DateTime.Parse(searchViewModel.ToDate.ToString()).ToShortDateString()));
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }

            }

            // Pagination
            string paginationClause = null;
            if (!searchViewModel.IsDownload)
            {
                

            int pageSize = ConstantHelper.PaginationPageSize;
            int fromRowNum = ((searchViewModel.Page ?? 1) - 1) * pageSize;
            ParameterValues.Add(new SqlParameter("FromRowNum", fromRowNum));
            ParameterValues.Add(new SqlParameter("PageSize", pageSize));
            paginationClause = " OFFSET @FromRowNum ROWS FETCH NEXT @PageSize ROWS ONLY ";
            }

            // Order By
            string orderByClause = @" ORDER BY[TableCabinCrews].[Name] COLLATE Chinese_PRC_CI_AS,[Date] DESC ,  [TableEnglishTests].[Type] ";

            // Uploader ID
            string sqlString = "";
            string sqlStringTotal = "";

            // Build SQL
            if (searchViewModel.IsLatest)
            {
                StringBuilder sb = new StringBuilder();
                if (!String.IsNullOrEmpty(searchViewModel.CCName))
                {
                    sb.Append(" AND ").Append(ConstantHelper.TableNameCabinCrews + $".[Name] IN ({nameParamsStr}) ");
                }

                sqlString = SqlQueryHelper.GetSqlEnglishTestIsLatest(sb.ToString(), paginationClause);
                sqlStringTotal = SqlQueryHelper.GetSqlEnglishTestIsLatestTotal(sb.ToString());
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                if (ParameterNames.Any())
                {
                    sb.Append(" WHERE ");
                    sb.Append(String.Join(" AND ", ParameterNames));
                    sb.Append(" ");
                }

                sqlString = SqlQueryHelper.GetSqlEnglishTest(sb.ToString(), paginationClause, orderByClause, null);
                sqlStringTotal = SqlQueryHelper.GetSqlEnglishTestTotal(sb.ToString(), orderByClause);
            }
            if (!String.IsNullOrEmpty(searchViewModel.UploadRecordID))
            {
                StringBuilder sb = new StringBuilder();
                ParameterValues.Add(new SqlParameter("UploadRecordID", searchViewModel.UploadRecordID));
                sb.Append(" WHERE ").Append(ConstantHelper.TableNameEnglishTests + ".[UploadRecordID]=@UploadRecordID ");

                sqlString = SqlQueryHelper.GetSqlEnglishTest(sb.ToString(), paginationClause, orderByClause, null); ;
            }

            if (isToalNumber)
            {
                return sqlStringTotal;
            }
            return sqlString;
        }

        protected override IEnumerable<ISearchResultModel> GetResultBySqlQuery(string sqlString)
        {
            return DbManager.SqlQuery<SearchResult>(sqlString, ParameterValues.ToArray()).ToList();
        }

        protected override IEnumerable<ISearchResultModel> GetResultIsLatestBySqlQuery(string sqlString)
        {
            return DbManager.SqlQuery<SearchResultIsLatest>(sqlString, ParameterValues.ToArray()).ToList();
        }
    }
}