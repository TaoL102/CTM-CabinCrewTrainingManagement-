using System.ComponentModel.DataAnnotations;
using CTMLocalizationLib.Resources;

namespace CTM.Areas.Search.ViewModels
{
    public interface ISearchViewModel
    {
         int? Page { get; set; }
         bool IsLatest { get; set; }
         bool IsDownload { get; set; }
    }
}