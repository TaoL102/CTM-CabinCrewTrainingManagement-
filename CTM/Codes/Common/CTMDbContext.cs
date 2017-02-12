using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using CTM.Codes.Helpers;
using CTM.Models;
using EntityFramework.Reflection;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CTM.Codes.Common
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