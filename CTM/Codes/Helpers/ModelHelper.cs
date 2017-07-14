using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using CTM.Codes.Extensions;
using WebGrease.Css.Extensions;
using CTMLocalizationLib.Helpers;

namespace CTM.Codes.Helpers
{
    public static class ModelHelper
    {
        public static List<string> GetAllProperties(object obj,bool isCollectionNavProp = false)
        {
            return GetAllProperties(obj.GetType(),isCollectionNavProp);
        }

        public static List<string> GetAllProperties<T>(bool isCollectionNavProp = false)
        {
            return GetAllProperties(typeof(T), isCollectionNavProp);
        }

        public static List<string> GetAllProperties(Type type, bool isCollectionNavProp = false)
        {
            var properties =
                GetAllPropertyInfo(type)
                    .Where(o => o.PropertyType.IsNonStringEnumerable() == isCollectionNavProp)
                    .Select(o => o.Name)
                    .ToList();
            return properties;
        }

        public static List<PropertyInfo> GetAllPropertyInfo<T>()
        {
            return GetAllPropertyInfo(typeof(T));
        }

        public static List<PropertyInfo> GetAllPropertyInfo(object obj)
        {
            return GetAllPropertyInfo(obj.GetType());
        }

        public static List<PropertyInfo> GetAllPropertyInfo(Type type)
        {
            var properties = type
                .GetProperties()
                .ToList();
            return properties;
        }

        public static Dictionary<string, string> GetDisplayPropertyNameAndValues<T>(T obj)
        {
            var dic = new Dictionary<string, string>();

            // Get property names
            var list = GetAllDisplayProperties(obj.GetType());

            // Get property values
            list.ForEach(o =>
            {
                dic.Add(o.Name, GetPropertyValue(obj, o));
            });

            return dic;
        }

        public static List<string> GetDisplayPropertyValues(object obj)
        {
            List<string> displayValues = new List<string>();

            // Properties
            var allDisplayProperties = GetAllDisplayProperties(obj.GetType());

            // Values
            allDisplayProperties.ForEach(o =>
            {
                var str = GetPropertyValue(obj, o);
                displayValues.Add(str);
            });

            return displayValues;
        }

        public static List<PropertyInfo> GetAllDisplayProperties(Type type)
        {
            return GetAllPropertyInfo(type).Where(o => o.GetCustomAttribute<DisplayAttribute>() != null).ToList();
        }

        public static int GetDisplayPropertyCount(Type type)
        {
            return GetAllDisplayProperties(type).Count;
        }

        public static string GetPropertyValue(object obj, PropertyInfo propertyInfo)
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
            var propertyInfo = obj.GetType().GetProperty(propName);
            return GetPropertyValue(obj, propertyInfo);
        }

        public static string GetEnumPropertyValue(Enum enumValue)
        {
            try
            {
                return CTMLocalizationLib.Resources.ConstModels.ResourceManager.GetString(enumValue.ToString());
            }
            catch (Exception e)
            {
                return enumValue.ToString();
            }

        }

        public static List<string> GetAllPropertyValues(object obj, bool isCollectionNavProp = false)
        {
            List<string> list = new List<string>();

            var properties =
                GetAllPropertyInfo(obj)
                    .Where(o => o.PropertyType.IsNonStringEnumerable() == isCollectionNavProp)
                    .ToList();

            properties.ForEach(o =>
            {
                list.Add(GetPropertyValue(obj, o));
            });

            return list;
        }

        public static List<string> GetDisplayPropertyNames(object obj)
        {
            return GetDisplayPropertyNames(obj.GetType());
        }

        public static List<string> GetDisplayPropertyNames(Type type)
        {
            var allDisplayProperties = GetAllDisplayProperties(type);
            List<string> displayNames = new List<string>();
            allDisplayProperties.ForEach(o =>
            {
                var str = GetPropertyDisplayName(o);
                displayNames.Add(str);
            });
            return displayNames;
        }

        public static List<string> GetDisplayPropertyNames<T>()
        {           
            return GetDisplayPropertyNames(typeof(T));
        }

        public static string GetPropertyDisplayName<T>(string propName)
        {
            return GetPropertyDisplayName(typeof(T),propName);
        }

        public static string GetPropertyDisplayName(Type type,string propName)
        {
            var propertyInfo =type.GetProperty(propName);
            return GetPropertyDisplayName(propertyInfo);
        }

        public static string GetPropertyDisplayName(MemberInfo propertyInfo)
        {
            var propertyCustomName = (propertyInfo?.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute)?.Name;
            var localizedName = LocalizationHelper.GetModelString(propertyCustomName ?? propertyInfo?.Name);
            return localizedName ?? propertyInfo?.Name;
        }
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
            return ModelHelper.GetAllProperties<T>(isCollectionNavProp);
        }

        public static List<PropertyInfo> GetAllDisplayProperties()
        {
            return ModelHelper.GetAllDisplayProperties(typeof(T));
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
            return ModelHelper.GetAllPropertyInfo<T>();
        }

        public static string GetPropertyName(PropertyInfo propertyInfo)
        {
            return propertyInfo.Name;
        }

        public static List<string> GetPropertyDisplayNames()
        {
            var allDisplayProperties = GetAllDisplayProperties();
            List<string> displayNames = new List<string>();
            allDisplayProperties.ForEach(o =>
            {
                var str = ModelHelper.GetPropertyDisplayName(o);
                displayNames.Add(str);
            });
            return displayNames;
        }

        public static List<string> GetDisplayPropertyValues(object obj)
        {
            return ModelHelper.GetDisplayPropertyValues(obj);
        }




        public static string GetPropertyName<TValue>(Expression<Func<T, TValue>> expression)
        {
            // Reference: http://stackoverflow.com/questions/4606211/expressionfunctmodel-tvalue-how-can-i-get-tvalues-name
            var memberEx = expression.Body as MemberExpression;

            if (memberEx == null)
                throw new ArgumentException("Body not a member-expression.");

            return memberEx.Member.Name;
        }

        public static List<string> GetAllPropertyValues(object obj,bool isCollectionNavProp = false)
        {
            List<string> list=new List<string>();
            var properties =
                GetAllPropertyInfo()
                    .Where(o => o.PropertyType.IsNonStringEnumerable() == isCollectionNavProp)
                    .ToList();

            properties.ForEach(o=>
            {
                list.Add(GetPropertyValue(obj,o));
            });

            return list;
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
            return ModelHelper.GetPropertyValue(obj, propertyInfo);
        }



        public static string GetPropertyDisplayName(string propName)
        {
            return ModelHelper.GetPropertyDisplayName<T>(propName);
        }

        public static string GetPropertyDisplayName(MemberInfo propertyInfo)
        {
            return ModelHelper.GetPropertyDisplayName(propertyInfo);
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