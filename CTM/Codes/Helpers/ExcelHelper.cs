using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using CTM.Areas.ManageData.ViewModels.EnglishTests;
using CTM.Areas.Search.ViewModels;
using CTM.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Microsoft.Ajax.Utilities;

namespace CTM.Codes.Helpers
{
    public static class ExcelHelper
    {
        private static CultureInfo culture = new CultureInfo("zh-CN");

        /// <summary>
        /// Generate an excel by a contents of objects
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static Stream GenerateExcel<T>(string fileName,IList<T> list)
        {
            // header
            var header = ModelHelper.GetDisplayPropertyNames(list.FirstOrDefault()?.GetType());

            return GenerateExcel<T>(fileName, header, list);
        }

        public static Stream GenerateExcel<T>(string filename, List<string> header, IList<T> list)
        {
            // content
            var content = new List<object[]>();
            list.ForEach(o =>
            {
                string[] array = ModelHelper.GetDisplayPropertyValues(o).ToArray();
                content.Add(array);
            });

            // Return a MemoryStream 
            return GenerateExcel(filename, header, content,null);
        }

        public static Stream GenerateExcel(string filename, List<string> header, IList<object[]> contents,  Stream stream = null)
        {
            using (var excelPackage = new ExcelPackage(stream ?? new MemoryStream()))
            {
                // New worksheet
                excelPackage.Workbook.Worksheets.Add(filename);
                var worksheet = excelPackage.Workbook.Worksheets[1];

                // Number of columns
                var numOfCol = header.Count;

                // Format the header    
                using (ExcelRange objRange = worksheet.Cells[1, 1, 1, numOfCol])
                {
                    objRange.Style.Font.Bold = true;
                    objRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    objRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    objRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    objRange.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                }

                // Header
                for (int i = 0; i < numOfCol; i++)
                {
                    worksheet.Cells[1, i + 1].Value = header[i];
                }

                // Content
                worksheet.Cells["A2"].LoadFromArrays(contents);
                worksheet.Cells.AutoFitColumns();

                // Save
                excelPackage.Save();

                return excelPackage.Stream;
            }
        }

        public static List<List<string>> ReadFromExcel(Stream stream,int colNumber)
        {
            var list = new List<List<string>>();
            using (ExcelPackage excel = new ExcelPackage(stream))
            {
                var workSheet = excel.Workbook.Worksheets.First();

                for (var i = 2; i <= workSheet.Dimension.End.Row; i++)
                {
                    var strList = new List<string>();
                    for (var j = 1; j <= colNumber; j++)
                    {
                        strList.Add(workSheet.Cells[i, j].Text);
                    } 
                    list.Add(strList);
                }
            }
            return list;
        }
        public static List<CabinCrew> GenerateListCabinCrewFromExcel(Stream stream)
        {
            var cabinCrewsInUpload = new List<CabinCrew>();

            using (ExcelPackage excel = new ExcelPackage(stream))
            {
                var workSheet = excel.Workbook.Worksheets.First();

                for (var i = 2; i <= workSheet.Dimension.End.Row; i++)
                {
                    if (string.IsNullOrEmpty(workSheet.Cells[i, 1].Text) ||
                        string.IsNullOrEmpty(workSheet.Cells[i, 2].Text)
                    )
                    {
                        continue;
                    }
                    cabinCrewsInUpload.Add(
                        new CabinCrew()
                        {
                            Name = workSheet.Cells[i, 1].Text,
                            ID = workSheet.Cells[i, 2].Text,
                            IsResigned = false
                        }
                    );
                }
            }

            return cabinCrewsInUpload;
        }




        public static List<RefresherTraining> GenerateListRefresherTrainingFromExcel(Stream stream, DateTime date, string CategoryID, string uploadRecordID)
        {

            var list = new List<RefresherTraining>();

            using (ExcelPackage excel = new ExcelPackage(stream))
            {
                var workSheet = excel.Workbook.Worksheets.First();

                for (var i = 2; i <= workSheet.Dimension.End.Row; i++)
                {
                    if (string.IsNullOrEmpty(workSheet.Cells[i, 1].Text) ||
                        string.IsNullOrEmpty(workSheet.Cells[i, 2].Text) ||
                        string.IsNullOrEmpty(workSheet.Cells[i, 3].Text) ||
                        string.IsNullOrEmpty(workSheet.Cells[i, 4].Text)
                    )
                    {
                        continue;
                    }


                    list.Add(
                        new RefresherTraining()
                        {
                            ID = Guid.NewGuid().ToString(),
                            CabinCrewID = workSheet.Cells[i, 1].Text,
                            Date = date.ToUniversalTime().Date,
                            CategoryID = CategoryID,
                            UploadRecordID = uploadRecordID,
                            Remark = workSheet.Cells[i, 3].Text,
                        }
                    );
                }
            }

            return list;
        }



     

        public static bool CheckIsExcel(HttpPostedFileBase postedFile)
        {

            //-------------------------------------------
            //  Check the excel extension
            //-------------------------------------------
            if (Path.GetExtension(postedFile.FileName).ToLower() != ".xls"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".xlsx")
            {
                return false;
            }

            //-------------------------------------------
            //  Attempt to read the file and check the first bytes
            //-------------------------------------------
            try
            {
                if (!postedFile.InputStream.CanRead)
                {
                    return false;
                }

                if (postedFile.ContentLength < 512)
                {
                    return false;
                }

            }
            catch (Exception)
            {
                return false;
            }



            return true;
        }

        public static Stream GenerateRefresherTrainingsTemplate(List<CabinCrew> listCabinCrews, Stream stream = null)
        {
            // get cabin crew name and id contents
            //var listCabinCrews = Queryable.Select(db.CabinCrews, c => new { c.ID, Name = c.Name, c.IsResigned }).ToList();
            var list = listCabinCrews.Where(c => c.IsResigned == false)
                .Select(c => new { c.ID, c.Name })
                .OrderBy(o => o.Name, StringComparer.Create(culture, false));

            using (var excelPackage = new ExcelPackage(stream ?? new MemoryStream()))
            {
                // new worksheet
                excelPackage.Workbook.Worksheets.Add("Refresher Training Template");
                var worksheet = excelPackage.Workbook.Worksheets[1];

                //Format the header    
                using (ExcelRange objRange = worksheet.Cells["A1:C1"])
                {
                    objRange.Style.Font.Bold = true;
                    objRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    objRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    objRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    objRange.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                }
                worksheet.Cells["A1"].Value = "员工ID";
                worksheet.Cells["B1"].Value = "姓名";
                worksheet.Cells["C1"].Value = "备注";

                // write contents to worksheet
                worksheet.Cells["A2"].LoadFromCollection(list, false);
                worksheet.Cells.AutoFitColumns();

                // save
                excelPackage.Save();

                return excelPackage.Stream;
            }
        }
    }
}