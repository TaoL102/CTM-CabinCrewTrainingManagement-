using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CTM.Database;
using CTMLib.Helpers;
using CTMLib.Models;
using EntityFramework.BulkInsert.Extensions;

namespace CTM.Managers
{
    public class DbManager<T> where T : class
    {
        private readonly CTMDbContext _db = new CTMDbContext();

        public IQueryable<EnglishTest> EnglishTests
        {
            get { return DbSet<EnglishTest>(); }
        }

        public IQueryable<Category> Categories
        {
            get { return DbSet<Category>(); }
        }

        public IQueryable<CabinCrew> CabinCrews
        {
            get { return DbSet<CabinCrew>(); }
        }

        public IQueryable<UploadRecord> UploadRecords
        {
            get { return DbSet<UploadRecord>(); }
        }

        public IQueryable<RefresherTraining> RefresherTrainings
        {
            get { return DbSet<RefresherTraining>(); }
        }

        public IQueryable<Log> Logs
        {
            get { return DbSet<Log>(); }
        }

        public async Task<TElement> Add<TElement>(TElement entity) where TElement : class
        {
            DbSet<Log>().Add(GenerateChangeLog(entity, null, EntityState.Added));

            return DbSet<TElement>().Add(entity);
        }

        public void AddRange<TElement>(List<TElement> entities)
        {
            //Log
            entities.ForEach(o =>
            {
                object nullValue = null;
                DbSet<Log>().Add(GenerateChangeLog(o, nullValue, EntityState.Added));
            });

            _db.BulkInsert(entities);
        }

     public async Task Remove<TElement>(string id) where TElement : class
        {
            // Log
            var dbEntity = await GetEntityAsync<TElement>(id);
            DbSet<Log>().Add(GenerateChangeLog(dbEntity, null, EntityState.Deleted));

            DbSet<TElement>().Remove(dbEntity);
        }

        public async Task Update<TElement>(TElement entity) where TElement : class
        {
            var curEntity = entity;
            var oriEntity = await GetEntityAsync(curEntity);
            var navProp = ModelHelper<TElement>.GetNavProperties();
            navProp.ForEach(o =>
            {
                var value = oriEntity.GetType().GetProperty(o).GetValue(oriEntity);
                curEntity.GetType().GetProperty(o).SetValue(curEntity,value);
            });

            DbSet<Log>().Add(GenerateChangeLog(curEntity, oriEntity, EntityState.Modified));

            DbSet<TElement>().AddOrUpdate(curEntity);
        }

        public async Task<TElement> FindAsync<TElement>(params object[] keyValues) where TElement : class
        {
            return await DbSet<TElement>().FirstAsync();
        }

        public DbRawSqlQuery<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            return _db.Database.SqlQuery<TElement>(sql, parameters);
        }

        public CTMDbContext GetContext()
        {
            return _db;
        }

        public IQueryable<TElement> DbSet<TElement>(bool isLazyLoading=false) where TElement : class
        {
            var query = Queryable.AsQueryable<TElement>(_db.Set<TElement>());

            if (isLazyLoading)
            {
                return query;
            }

            // Include navigation properties
            var navigationProperties = ModelHelper<TElement>.GetNavProperties();

            query = navigationProperties.Aggregate<string, IQueryable<TElement>>
     (query, (current, expression) => current.Include(expression));

            return query;
        }

        public DbSet<TElement> DbSet<TElement>() where TElement : class
        {
            return _db.Set<TElement>();
        }

        public async Task<int> SaveChangesAsync()
        {

            return await _db.SaveChangesAsync();
        }

        public async Task<TElement> GetEntityAsync<TElement>(params object[] keyValues) where TElement : class
        {
            var query = Queryable.AsQueryable<TElement>(_db.Set<TElement>());

            // Where Id
            var idLamdaExprs = ModelHelper<TElement>.GetIdWhereClauseLamdaExpressions(keyValues);
            query = idLamdaExprs.Aggregate<Expression<Func<TElement, bool>>, IQueryable<TElement>>
        (query, (current, expression) => current.Where(expression));

            // Include navigation properties
            var navigationProperties = ModelHelper<TElement>.GetNavProperties();

            query = navigationProperties.Aggregate<string, IQueryable<TElement>>
     (query, (current, expression) => current.Include(expression));

            var dbEntity = await query.SingleOrDefaultAsync();
            return dbEntity;
        }

        public async Task<TElement> GetEntityAsync<TElement>(TElement entity, bool isLazyLoading = false) where TElement : class
        {
            var query = Queryable.AsQueryable<TElement>(_db.Set<TElement>());

            // Where Id
            var idLamdaExprs = ModelHelper<TElement>.GetIdWhereClauseLamdaExpressions(entity);
            query = idLamdaExprs.Aggregate<Expression<Func<TElement, bool>>, IQueryable<TElement>>
        (query, (current, expression) => current.Where(expression));

            if (isLazyLoading)
            {
                return await query.SingleOrDefaultAsync();
            }

            // Include navigation properties
            var navigationProperties = ModelHelper<TElement>.GetNavProperties();

            query = navigationProperties.Aggregate<string, IQueryable<TElement>>
     (query, (current, expression) => current.Include(expression));

            var dbEntity = await query.SingleOrDefaultAsync();
            return dbEntity;

        }



        #region Private Methods


        private  Log GenerateChangeLog<TElement>(TElement curEntity, TElement oriEntity, EntityState entityState) where TElement : class
        {

            var userId = System.Web.HttpContext.Current.User.Identity.Name;
            string tableName = ModelHelper<TElement>.GetModelName();
            string description = null;
            LogEventType eventType = LogEventType.Add;

            switch (entityState)
            {
                case EntityState.Added:
                    eventType = LogEventType.Add;
                    description = curEntity.ToString();
                    break;
                case EntityState.Deleted:
                    eventType = LogEventType.Delete;
                    description = curEntity.ToString();
                    break;
                case EntityState.Modified:
                    eventType = LogEventType.Edit;
                    description = "Original:" + oriEntity.ToString() + "\nCurrent:" + curEntity.ToString();
                    break;
            }

            return new Log()
            {
                EventType = eventType,
                TableName = tableName,
                Description = description,
                UserId = userId,
            };

        }

        public void Dispose()
        {
            _db.Dispose();
        }

        #endregion
    }

}