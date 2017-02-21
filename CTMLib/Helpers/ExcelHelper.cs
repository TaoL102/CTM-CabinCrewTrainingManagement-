using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using CTMLib.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace CTMLib.Helpers
{
    public static class ExcelHelper
    {
        private static  CultureInfo culture = new CultureInfo("zh-CN");
        

        public static List<CabinCrew> GenerateListCabinCrewFromExcel(Stream stream)
        {
            var cabinCrewsInUpload = new List<CabinCrew>();

            using (ExcelPackage excel = new ExcelPackage(stream))
            {
                var workSheet = excel.Workbook.Worksheets.First();

                for (var i = 2; i <= workSheet.Dimension.End.Row; i++)
                {
                    if (string.IsNullOrEmpty(workSheet.Cells[i, 1].Text)||
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

        public static List<EnglishTest> GenerateListEnglishTestFromExcel(Stream stream, DateTime date, string englishTestCategoryID,string uploadRecordID)
        {

            var list = new List<EnglishTest>();

            using (ExcelPackage excel = new ExcelPackage(stream))
            {
                var workSheet = excel.Workbook.Worksheets.First();

                for (var i = 2; i <= workSheet.Dimension.End.Row; i++)
                {
                    var cell1 = workSheet.Cells[i, 1].Text;
                    var cell2 = workSheet.Cells[i, 2].Text;
                    var cell3 = workSheet.Cells[i, 3].Text;
                    var cell4 = workSheet.Cells[i, 4].Text;

                    if (string.IsNullOrEmpty(cell1) ||  string.IsNullOrEmpty(cell2) )
                    {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(cell3))
                    {
                        list.Add(
                        // Announcement grade
                        new EnglishTest()
                        {
                            ID = Guid.NewGuid().ToString(),
                            CabinCrewID = workSheet.Cells[i, 1].Text,
                            Grade = workSheet.Cells[i, 3].Text,
                            Type = EnglishTestType.CabinAnnoucement,
                            Date = date.ToUniversalTime().Date,
                            CategoryID = englishTestCategoryID,
                            UploadRecordID = uploadRecordID,
                        }
                        );
                    }

                    if (!string.IsNullOrEmpty(cell4))
                    {
                        list.Add(
                       // Oral English grade
                       new EnglishTest()
                       {
                           ID = Guid.NewGuid().ToString(),
                           CabinCrewID = workSheet.Cells[i, 1].Text,
                           Grade = workSheet.Cells[i, 4].Text,
                           Type = EnglishTestType.SpokenSkill,
                           Date = date.ToUniversalTime().Date,
                           CategoryID = englishTestCategoryID,
                           UploadRecordID = uploadRecordID,
                       }
                       );
                    }
                    
                }
            }

            return list;
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

        public static Stream GenerateEnglishTestTemplate(List<CabinCrew> listCabinCrews, Stream stream = null)
        {
            // get cabin crew name and id list
           // var listCabinCrews = Queryable.Select(db.CabinCrews, c => new { c.ID, Name = c.Name,c.IsResigned }).ToList();
            var list= listCabinCrews.Where(c => c.IsResigned == false)
                .Select(c => new { c.ID, c.Name })
                .OrderBy(o => o.Name, StringComparer.Create(culture, false));

            using (var excelPackage = new ExcelPackage(stream ?? new MemoryStream()))
            {
                // new worksheet
                excelPackage.Workbook.Worksheets.Add("English Test Template");
                var worksheet = excelPackage.Workbook.Worksheets[1];

                //Format the header    
                using (ExcelRange objRange = worksheet.Cells["A1:D1"])
                {
                    objRange.Style.Font.Bold = true;
                    objRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    objRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    objRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    objRange.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                }
                worksheet.Cells["A1"].Value = "员工ID";
                worksheet.Cells["B1"].Value = "姓名";
                worksheet.Cells["C1"].Value = "CabinAnnoucement";
                worksheet.Cells["D1"].Value = "SpokenSkill";

                // write list to worksheet
                worksheet.Cells["A2"].LoadFromCollection(list, false);
                worksheet.Cells.AutoFitColumns();

                // save
                excelPackage.Save();

                return excelPackage.Stream;
            }
        }

        public static Stream GenerateExcel(string filename,IEnumerable<object[]> list ,List<string> headerList , Stream stream = null)
        {

            using (var excelPackage = new ExcelPackage(stream ?? new MemoryStream()))
            {
                // new worksheet
                excelPackage.Workbook.Worksheets.Add(filename);
                var worksheet = excelPackage.Workbook.Worksheets[1];

                // Get how many columns
                var numOfCol = headerList.Count;

                //Format the header    
                using (ExcelRange objRange = worksheet.Cells[1, 1,1,numOfCol])
                {
                    objRange.Style.Font.Bold = true;
                    objRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    objRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    objRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    objRange.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                }

                // Header
                for (int i=0;i< numOfCol;i++)
                {
                    worksheet.Cells[1,i+1].Value = headerList[i];
                }

                

                // write list to worksheet
                worksheet.Cells["A2"].LoadFromArrays(list);
                worksheet.Cells.AutoFitColumns();

                // save
                excelPackage.Save();

                return excelPackage.Stream;
            }
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
            // get cabin crew name and id list
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

                // write list to worksheet
                worksheet.Cells["A2"].LoadFromCollection(list, false);
                worksheet.Cells.AutoFitColumns();

                // save
                excelPackage.Save();

                return excelPackage.Stream;
            }
        }
    }
}