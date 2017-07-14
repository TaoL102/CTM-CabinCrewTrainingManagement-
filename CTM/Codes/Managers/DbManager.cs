using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CTM.Codes.Database;
using CTM.Codes.Helpers;
using CTM.Models;
using EntityFramework.BulkInsert.Extensions;

namespace CTM.Codes.Managers
{
    public class DbManager
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

        public async Task<T> Add<T>(T entity) where T : class
        {
            DbSet<Log>().Add(GenerateChangeLog(entity, null, EntityState.Added));

            return DbSet<T>().Add(entity);
        }

        public void AddRange<T>(List<T> entities)
        {
            //Log
            entities.ForEach(o =>
            {
                object nullValue = null;
                DbSet<Log>().Add(GenerateChangeLog(o, nullValue, EntityState.Added));
            });

            _db.BulkInsert(entities);
        }

     public async Task Remove<T>(string id) where T : class
        {
            // Log
            var dbEntity = await GetEntityAsync<T>(id);
            DbSet<Log>().Add(GenerateChangeLog(dbEntity, null, EntityState.Deleted));

            DbSet<T>().Remove(dbEntity);
        }

        public async Task Update<T>(T entity) where T : class
        {
            var curEntity = entity;
            var oriEntity = await GetEntityAsync(curEntity);
            var navProp = ModelHelper<T>.GetNavProperties();
            navProp.ForEach(o =>
            {
                var value = oriEntity.GetType().GetProperty(o).GetValue(oriEntity);
                curEntity.GetType().GetProperty(o).SetValue(curEntity,value);
            });

            DbSet<Log>().Add(GenerateChangeLog(curEntity, oriEntity, EntityState.Modified));

            DbSet<T>().AddOrUpdate(curEntity);
        }

        public async Task<T> FindAsync<T>(params object[] keyValues) where T : class
        {
            return await DbSet<T>().FirstAsync();
        }

        public DbRawSqlQuery<T> SqlQuery<T>(string sql, params object[] parameters)
        {
            return _db.Database.SqlQuery<T>(sql, parameters);
        }

        public CTMDbContext GetContext()
        {
            return _db;
        }

        public IQueryable<T> DbSet<T>(bool isLazyLoading=false) where T : class
        {
            var query = Queryable.AsQueryable<T>(_db.Set<T>());

            if (isLazyLoading)
            {
                return query;
            }

            // Include navigation properties
            var navigationProperties = ModelHelper<T>.GetNavProperties();

            query = navigationProperties.Aggregate<string, IQueryable<T>>
     (query, (current, expression) => current.Include(expression));

            return query;
        }

        public DbSet<T> DbSet<T>() where T : class
        {
            return _db.Set<T>();
        }

        public async Task<int> SaveChangesAsync()
        {

            return await _db.SaveChangesAsync();
        }

        public async Task<T> GetEntityAsync<T>(params object[] keyValues) where T : class
        {
            var query = Queryable.AsQueryable<T>(_db.Set<T>());

            // Where Id
            var idLamdaExprs = ModelHelper<T>.GetIdWhereClauseLamdaExpressions(keyValues);
            query = idLamdaExprs.Aggregate<Expression<Func<T, bool>>, IQueryable<T>>
        (query, (current, expression) => current.Where(expression));

            // Include navigation properties
            var navigationProperties = ModelHelper<T>.GetNavProperties();

            query = navigationProperties.Aggregate<string, IQueryable<T>>
     (query, (current, expression) => current.Include(expression));

            var dbEntity = await query.SingleOrDefaultAsync();
            return dbEntity;
        }

        public async Task<T> GetEntityAsync<T>(T entity, bool isLazyLoading = false) where T : class
        {
            var query = Queryable.AsQueryable<T>(_db.Set<T>());

            // Where Id
            var idLamdaExprs = ModelHelper<T>.GetIdWhereClauseLamdaExpressions(entity);
            query = idLamdaExprs.Aggregate<Expression<Func<T, bool>>, IQueryable<T>>
        (query, (current, expression) => current.Where(expression));

            if (isLazyLoading)
            {
                return await query.SingleOrDefaultAsync();
            }

            // Include navigation properties
            var navigationProperties = ModelHelper<T>.GetNavProperties();

            query = navigationProperties.Aggregate<string, IQueryable<T>>
     (query, (current, expression) => current.Include(expression));

            var dbEntity = await query.SingleOrDefaultAsync();
            return dbEntity;

        }



        #region Private Methods


        private  Log GenerateChangeLog<T>(T curEntity, T oriEntity, EntityState entityState) where T : class
        {

            var userId = System.Web.HttpContext.Current.User.Identity.Name;
            string tableName = ModelHelper<T>.GetModelName();
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