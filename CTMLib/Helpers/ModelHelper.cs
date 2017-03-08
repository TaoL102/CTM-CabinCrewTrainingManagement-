using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using CTMLib.Extensions;
using WebGrease.Css.Extensions;

namespace CTMLib.Helpers
{
    public static class ModelHelper
    {

    }

    public static class ModelHelper<T> 
    {

        public static List<string> GetNavProperties(bool isCollection = false)
        {
            var properties =
                GetAllPropertyInfo()
                 .Where(o => o.GetAccessors().Any(a => a.IsVirtual) && o.PropertyType.IsNonStringEnumerable() == isCollection)
                 .Select(o => o.Name)
                 .ToList();
            return properties;
        }

        public static List<string> GetAllProperties(bool isCollectionNavProp = false)
        {
            var properties =
                  GetAllPropertyInfo()
                  .Where(o=>o.PropertyType.IsNonStringEnumerable()==isCollectionNavProp)
                 .Select(o => o.Name)
                 .ToList();
            return properties;
        }

        public static List<string> GetNonNavproperties()
        {
            var properties =
                  GetAllPropertyInfo()
                 .Where(o => o.GetAccessors().Any(a => a.IsVirtual == false))
                 .Select(o => o.Name)
                 .ToList();
            return properties;
        }

        public static List<PropertyInfo> GetPrimaryKeys()
        {
            var properties =
                  GetAllPropertyInfo()
                 .Where(o => o.CustomAttributes.Any(t => t.AttributeType == typeof(KeyAttribute)))
                 .ToList();
            return properties;
        }

        public static string[] GetPrimaryKeyValues(T entity)
        {
            var values = new List<string>();
            var keys = GetPrimaryKeys();
            keys.ForEach(o =>
            {
                values.Add(GetPropertyValue(entity, o).ToString());
            });

            return values.ToArray();
        }

        public static Dictionary<string, string> GetPrimaryKeysAndValues(T entity)
        {
            var dic = new Dictionary<string,string>();
            var keys = GetPrimaryKeys();
            keys.ForEach(o =>
            {
                dic.Add(
                    GetPropertyName(o),
                    GetPropertyValue(entity, o).ToString()
                    );
            });

            return dic;
        }

        public static Dictionary<string, string> GetPrimaryKeysAndValues(params object[] keyValues)
        {
            var dic = new Dictionary<string, string>();
            var keys = GetPrimaryKeys();
            var valuesCount = keyValues.Length;

            for (int i=0;i<keys.Count;i++)
            {
                if (i < valuesCount)
                {
                    dic.Add(GetPropertyName(keys[i]), keyValues[i].ToString());
                }
            }

            return dic;
        }


        public static List<Expression<Func<T, bool>>> GetIdWhereClauseLamdaExpressions(T entity)
        {
            var idLamdaExpr = new List<Expression<Func<T, bool>>>();
            GetPrimaryKeysAndValues(entity).ForEach(o =>
            {
                idLamdaExpr.Add(GetLamdaExpressionBool(o.Key, o.Value));
            });

            return idLamdaExpr;
        }

        public static List<Expression<Func<T, bool>>> GetIdWhereClauseLamdaExpressions(params object[] keyValues)
        {
            var idLamdaExpr = new List<Expression<Func<T, bool>>>();
            GetPrimaryKeysAndValues(keyValues).ForEach(o =>
            {
                idLamdaExpr.Add(GetLamdaExpressionBool(o.Key, o.Value));
            });

            return idLamdaExpr;
        }

        public static Expression<Func<T,bool>> GetLamdaExpressionBool(string propertyName,string propertyValue)
        {
            var parameterExp = Expression.Parameter(ObjectContext.GetObjectType(typeof(T)),"type");
            var propertyExp = Expression.Property(parameterExp, propertyName);
            MethodInfo method = typeof(string).GetMethod("Equals", new[] { typeof(string) });
            var someValue = Expression.Constant(propertyValue, typeof(string));
            var containsMethodExp = Expression.Call(propertyExp, method, someValue);

            return Expression.Lambda<Func<T, bool>>
                         (containsMethodExp, parameterExp);
        }

        public static List<PropertyInfo> GetAllPropertyInfo()
        {
            var properties = typeof(T)
                 .GetProperties()
                 .ToList();
            return properties;
        }

        public static string GetPropertyName(PropertyInfo propertyInfo)
        {
            return propertyInfo.Name;
        }

        public static string GetPropertyDisplayName(MemberInfo propertyInfo)
        {
            var propertyCustomName = (propertyInfo?.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute)?.Name;
             var localizedName = LocalizationHelper.GetModelString(propertyCustomName ?? propertyInfo?.Name);
            return localizedName??propertyInfo?.Name;

        }

        public static string GetPropertyDisplayName(string propName)
        {
            var propertyInfo = typeof(T).GetProperty(propName);
            return GetPropertyDisplayName(propertyInfo);

        }

        public static string GetPropertyName<TValue>(Expression<Func<T, TValue>> expression)
        {
            // Reference: http://stackoverflow.com/questions/4606211/expressionfunctmodel-tvalue-how-can-i-get-tvalues-name
            var memberEx = expression.Body as MemberExpression;

            if (memberEx == null)
                throw new ArgumentException("Body not a member-expression.");

            return memberEx.Member.Name;
        }

        public static string GetPropertyDisplayValue<TValue>(Expression<Func<T, TValue>> expression,object obj)
        {
            object value = null;
            var memberEx = expression.Body as MemberExpression;

            if ((memberEx?.Member as PropertyInfo) != null)
            {
                value = GetPropertyValue(obj, (PropertyInfo) memberEx.Member);
            }

            return value?.ToString();
        }

        public static string GetPropertyValue(object obj,PropertyInfo propertyInfo)
        {
            var value = propertyInfo.GetValue(obj);
            if (value is Enum)
            {
                return GetEnumPropertyValue(value as Enum);
            }
            return value?.ToString();
        }


        public static string GetPropertyValue(object obj, string propName)
        {
            var propertyInfo = typeof(T).GetProperty(propName);
            return GetPropertyValue(obj,propertyInfo);
        }

        public static string GetEnumPropertyValue(Enum enumValue)
        {
            try
            {
                return Resources.ConstModels.ResourceManager.GetString(enumValue.ToString());
            }
            catch (Exception e)
            {
                return enumValue.ToString();
            }

        }

        public static string GetModelName()
        {
            TableAttribute tableAttr = typeof(T).GetCustomAttributes(typeof(TableAttribute), false).SingleOrDefault() as TableAttribute;

            // Get table name (if it has a Table attribute, use that, otherwise get the pluralized name)
           return tableAttr != null ? tableAttr.Name : GetModelType().Name;
        }

        public static Type GetModelType()
        {
           return ObjectContext.GetObjectType(typeof(T));
        }


    }

 }