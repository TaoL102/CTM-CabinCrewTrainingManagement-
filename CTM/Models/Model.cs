using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Reflection;
using CTM.Codes.Extensions;
using CTM.Codes.Helpers;

namespace CTM.Models
{
    public class Model :IModel
    {
        public override string ToString()
        {
            var properties = ModelHelper.GetAllPropertyInfo(this.GetType());
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

            var propertyName = ModelHelper.GetPropertyDisplayName(this.GetType(),propName);
            var propertyValue = value;

            return propertyName + ":" + (propertyValue ?? "Null");
        }

        private string PropertyToString(PropertyInfo propertyInfo)
        {
            var propertyName = ModelHelper.GetPropertyDisplayName(propertyInfo);
            var propertyValue = ModelHelper.GetPropertyValue(this, propertyInfo);

            return propertyName + ":" + (propertyValue ?? "Null");
        }


    }
}