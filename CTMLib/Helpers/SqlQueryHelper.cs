namespace CTMLib.Helpers
{
    public static class SqlQueryHelper
    {
        private static readonly string TableNameRefresherTrainings = ConstantHelper.TableNameRefresherTrainings;
        private static readonly string TableNameCabinCrews = ConstantHelper.TableNameCabinCrews;
        private static readonly string TableNameCategories = ConstantHelper.TableNameCategories;
        private static readonly string TableNameEnglishTests = ConstantHelper.TableNameEnglishTests;


        public static string GetSqlRefresherTraining(string whereClause, string rowNumberSql = null)
        {
            rowNumberSql = (rowNumberSql != null) ? ("," + rowNumberSql) : null;
            return @"
     SELECT " + TableNameRefresherTrainings + @".[ID] AS[ID],
            " + TableNameRefresherTrainings + @".[CabinCrewID] AS[CabinCrewID],
            " + TableNameRefresherTrainings + @".[CategoryID] AS[CategoryID],
            " + TableNameRefresherTrainings + @".[Remark] AS[Remark],
            " + TableNameRefresherTrainings + @".[Date] AS[Date],
            " + TableNameRefresherTrainings + @".[UploadRecordID] AS[UploadRecordID],
            " + TableNameCabinCrews + @".[Name] AS[CabinCrewName],
            " + TableNameCategories + @".[Name] AS[CategoryName] "
            + rowNumberSql + @"          
            FROM[dbo].[RefresherTrainings] AS" + TableNameRefresherTrainings + @"
            LEFT OUTER JOIN[dbo].[CabinCrews] AS" + TableNameCabinCrews + @" ON" + TableNameRefresherTrainings + @".[CabinCrewID] = " + TableNameCabinCrews + @".[ID]
            LEFT OUTER JOIN[dbo].[categories] AS" + TableNameCategories + @" ON" + TableNameRefresherTrainings + @".[categoryID] =" + TableNameCategories + @".[ID] "
            + whereClause;
        }

        public static string GetSqlRefresherTrainingIsLatest(string whereClause)
        {
            string rowNumberSql = @"ROW_NUMBER() OVER
         (
             PARTITION BY" + TableNameCabinCrews + @".[Name]
             ORDER BY CONVERT(datetime, " + TableNameRefresherTrainings + @".[Date], 101) DESC
         ) AS Recency";

            return
            @"WITH TABLE1 AS ( " + GetSqlRefresherTraining(whereClause, rowNumberSql) + ")" +
            @"
            SELECT *
            FROM TABLE1
            WHERE Recency = 1";
        }


        public static string GetSqlEnglishTest(string whereClause, string paginationClause = null, string orderByClause = null, string rowNumberSql = null)
        {
            rowNumberSql = (rowNumberSql != null) ? ("," + rowNumberSql) : null;
            return @"
            SELECT 
                " + TableNameEnglishTests + @".[CabinCrewID] AS [CabinCrewID], 
                " + TableNameEnglishTests + @".[Type] AS [Type], 
                " + TableNameEnglishTests + @".[ID] AS [ID], 
                " + TableNameCabinCrews + @".[Name] AS [CabinCrewName], 
                " + TableNameEnglishTests + @".[Grade] AS [Grade], 
                " + TableNameEnglishTests + @".[Date] AS [Date], 
                " + TableNameCategories + @".[Name] AS [CategoryName] "
                         + rowNumberSql + @"          
                FROM   [dbo].[EnglishTests] AS " + TableNameEnglishTests + @" 
                LEFT OUTER JOIN [dbo].[CabinCrews] AS " + TableNameCabinCrews + @" ON " + TableNameEnglishTests + @".[CabinCrewID] = " + TableNameCabinCrews + @".[ID]
                LEFT OUTER JOIN [dbo].[Categories] AS " + TableNameCategories + @" ON " + TableNameEnglishTests + @".[CategoryID] = " + TableNameCategories + @".[ID] "
                         + whereClause 
                         + orderByClause
                         + paginationClause;

        }

        public static string GetSqlEnglishTestTotal(string whereClause, string orderByClause = null, string rowNumberSql = null)
        {
            rowNumberSql = (rowNumberSql != null) ? ("," + rowNumberSql) : null;
            return @"SELECT COUNT(*) FROM
		   (
            SELECT 
                " + TableNameEnglishTests + @".[CabinCrewID] AS [CabinCrewID], 
                " + TableNameEnglishTests + @".[Type] AS [Type], 
                " + TableNameEnglishTests + @".[ID] AS [ID], 
                " + TableNameCabinCrews + @".[Name] AS [CabinCrewName], 
                " + TableNameEnglishTests + @".[Grade] AS [Grade], 
                " + TableNameEnglishTests + @".[Date] AS [Date], 
                " + TableNameCategories + @".[Name] AS [CategoryName] "
                   + rowNumberSql + @"          
                FROM   [dbo].[EnglishTests] AS " + TableNameEnglishTests + @" 
                LEFT OUTER JOIN [dbo].[CabinCrews] AS " + TableNameCabinCrews + @" ON " + TableNameEnglishTests +
                   @".[CabinCrewID] = " + TableNameCabinCrews + @".[ID]
                LEFT OUTER JOIN [dbo].[Categories] AS " + TableNameCategories + @" ON " + TableNameEnglishTests +
                   @".[CategoryID] = " + TableNameCategories + @".[ID] "
                   + whereClause
                   + @"
            )AS TABLE1";
        }

        public static string GetSqlEnglishTestIsLatest(string whereClause, string paginationClause = null)
        {
            string rowNumberSql = @"ROW_NUMBER() OVER
         (
             PARTITION BY " + TableNameCabinCrews + @".[Name]
             ORDER BY CONVERT(datetime, " + TableNameEnglishTests + @".[Date], 101) DESC
         ) AS Recency";

            return
            @"WITH SUBTABLE1 AS ( " +
            GetSqlEnglishTest("  WHERE  " + TableNameEnglishTests + @".[Type] =1 " + whereClause, null,null, rowNumberSql) +
            ")," +
            @"SUBTABLE2 AS ( " +
            GetSqlEnglishTest("  WHERE  " + TableNameEnglishTests + @".[Type] =2 " + whereClause, null,null, rowNumberSql) +
            ")" +

           @"SELECT  
			    ISNULL(SUBTABLE1.[CabinCrewID], SUBTABLE2.[CabinCrewID] )AS [CabinCrewID], 
			    ISNULL(SUBTABLE1.[CabinCrewName], SUBTABLE2.[CabinCrewName]) AS [CabinCrewName],
                SUBTABLE1.[Grade] AS [CabinAnnoucementGrade], 
                SUBTABLE1.[Date] AS [CabinAnnoucementDate], 
                SUBTABLE1.[CategoryName] AS [CabinAnnoucementCategoryName] ,
				SUBTABLE2.[Grade] AS [SpokenSkillGrade], 
                SUBTABLE2.[Date] AS [SpokenSkillDate], 
                SUBTABLE2.[CategoryName] AS [SpokenSkillCategoryName] 
			FROM SUBTABLE1 , SUBTABLE2
			WHERE SUBTABLE1.CABINCREWID=SUBTABLE2.CABINCREWID
			AND SUBTABLE1.RECENCY=1
			AND SUBTABLE2.RECENCY=1

            ORDER BY [SUBTABLE1].[CabinCrewName] COLLATE  Chinese_PRC_CI_AS 
            OFFSET @FromRowNum ROWS FETCH NEXT @PageSize ROWS ONLY "
            ;
        }

        public static string GetSqlEnglishTestIsLatestTotal(string whereClause)
        {
            string rowNumberSql = @"ROW_NUMBER() OVER
         (
             PARTITION BY " + TableNameCabinCrews + @".[Name]
             ORDER BY CONVERT(datetime, " + TableNameEnglishTests + @".[Date], 101) DESC
         ) AS Recency";

            return
            @"WITH SUBTABLE1 AS ( " +
            GetSqlEnglishTest("  WHERE  " + TableNameEnglishTests + @".[Type] =1 " + whereClause, null, null, rowNumberSql) +
            ")," +
            @"SUBTABLE2 AS ( " +
            GetSqlEnglishTest("  WHERE  " + TableNameEnglishTests + @".[Type] =2 " + whereClause, null, null, rowNumberSql) +
            ")" +

            @"
            SELECT  COUNT(*) 
			FROM SUBTABLE1 , SUBTABLE2
			WHERE SUBTABLE1.CABINCREWID=SUBTABLE2.CABINCREWID
			AND SUBTABLE1.RECENCY=1
			AND SUBTABLE2.RECENCY=1"
            ;
        }




    }
}