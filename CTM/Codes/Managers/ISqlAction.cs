using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTM.Areas.Search.ViewModels;

namespace CTM.Codes.Managers
{
    public interface ISqlAction
    {
        /// <summary>
        /// Generate Sql string by search view model
        /// </summary>
        /// <param name="searchViewModel"></param>
        /// <param name="isToalNumber"></param>
        /// <returns></returns>
        string GenerateSqlStringByFilter(ISearchViewModel searchViewModel, bool isToalNumber = false);

        /// <summary>
        /// Get total number of the results by search view model
        /// </summary>
        /// <param name="searchViewModel"></param>
        /// <returns></returns>
        int GetTotalNumberByFilter(ISearchViewModel searchViewModel);

        /// <summary>
        /// Get the search results list by search view model
        /// </summary>
        /// <param name="searchViewModel"></param>
        /// <returns></returns>
        IEnumerable<ISearchResultModel> GetResultByFilter(ISearchViewModel searchViewModel);

        /// <summary>
        /// Get the search results(latest) list by search view model
        /// </summary>
        /// <param name="searchViewModel"></param>
        /// <returns></returns>
        IEnumerable<ISearchResultModel> GetResultIsLatestByFilter(ISearchViewModel searchViewModel);
    }
}
