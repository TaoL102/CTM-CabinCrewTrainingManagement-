using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CTM.Areas.Search.ViewModels;

namespace CTM.Codes.Managers
{
    public abstract class SqlAction : ISqlAction
    {
        #region Fields

        protected readonly DbManager DbManager = new DbManager();

        #endregion

        #region Properties

        protected  List<object> ParameterValues { get; set; }
        protected  List<string> ParameterNames { get; set; }

        #endregion

        public abstract string GenerateSqlStringByFilter(ISearchViewModel searchViewModel, bool isToalNumber = false);

        public int GetTotalNumberByFilter(ISearchViewModel searchViewModel)
        {
            var sqlString = GenerateSqlStringByFilter(searchViewModel, true);

            // get total number 
            try
            {
                var totalNum = DbManager.SqlQuery<int>(sqlString, ParameterValues.ToArray()).First();
                return totalNum;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 0;
            }
        }

        public IEnumerable<ISearchResultModel> GetResultByFilter(ISearchViewModel searchViewModel)
        {
            // Generate sql
            var sqlString = GenerateSqlStringByFilter(searchViewModel, false);

            // Get data from database
            var list = GetResultBySqlQuery(sqlString);

            return list;
        }

        public IEnumerable<ISearchResultModel> GetResultIsLatestByFilter(ISearchViewModel searchViewModel)
        {
            // Generate sql
            var sqlString = GenerateSqlStringByFilter(searchViewModel,false);

            // get data from database    
            var list = GetResultIsLatestBySqlQuery(sqlString);

            return list;
        }

        protected abstract IEnumerable<ISearchResultModel> GetResultBySqlQuery(String sqlString);
        protected abstract IEnumerable<ISearchResultModel> GetResultIsLatestBySqlQuery(String sqlString);
    }
}