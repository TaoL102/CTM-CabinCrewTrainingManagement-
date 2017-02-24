using CTMLib.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CTM.Codes.Database
{
    public class CTMDbContext : IdentityDbContext<ApplicationUser>
    {


        #region Properties

        public System.Data.Entity.DbSet<EnglishTest> EnglishTests { get; set; }

        public System.Data.Entity.DbSet<Category> Categories { get; set; }

        public System.Data.Entity.DbSet<CabinCrew> CabinCrews { get; set; }

        public System.Data.Entity.DbSet<UploadRecord> UploadRecords { get; set; }

        public System.Data.Entity.DbSet<RefresherTraining> RefresherTrainings { get; set; }

        public System.Data.Entity.DbSet<Log> Logs { get; set; }

        #endregion

        #region Constructor

        public CTMDbContext()
    : base("CabinTrainingManagement", false)
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.AutoDetectChangesEnabled = false;
        }

        #endregion



        public static CTMDbContext Create()
        {
            return new CTMDbContext();
        }

    


    }
}