using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Reflection;
using System.Web;
using CTM.Codes.Extensions;
using CTM.Codes.Helpers;

namespace CTM.Models
{
    public class Model<T> where T : class
    {
        public override string ToString()
        {
            var properties = ModelHelper<T>.GetAllPropertyInfo();
            List<string> stringList = new List<string>();
            properties?.ForEach(o =>
                {
                    stringList.Add(PropertyToString(o));
                });

            return string.Join(";", stringList);
        }

        protected string PropertyToString(string propName,object value)
        {
            var type = ObjectContext.GetObjectType(value.GetType());
            if (type.IsEnum)
            {
                value = ((Enum) value).GetDisplayName();
            }

            var propertyName = ModelHelper<T>.GetPropertyDisplayName(propName);
            var propertyValue = value;

            return propertyName + ":" + (propertyValue ?? "Null");
        }

        private string PropertyToString(PropertyInfo propertyInfo)
        {
            var propertyName = ModelHelper<T>.GetPropertyDisplayName(propertyInfo);
            var propertyValue = ModelHelper<T>.GetPropertyValue(this, propertyInfo);

            return propertyName + ":" + (propertyValue ?? "Null");
        }


    }
}