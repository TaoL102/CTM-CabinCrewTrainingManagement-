using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace CTMLib.Helpers
{
    public static class LocalizationHelper
    {
        public static string GetModelString(string name)
        {
            return Resources.ConstModels.ResourceManager.GetString(name) ?? name;

        }

        public static string GetString(MemberInfo memberInfo)
        {
            var displayAttr = (memberInfo?.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute);
            if (displayAttr == null)
            {
                return memberInfo?.Name;
            }

            var localizedStr = LookupResource(displayAttr.ResourceType, displayAttr.Name);

            return localizedStr ?? memberInfo?.Name;
        }


        private static string LookupResource(Type resourceType, string resourceKey)
        {
            var propertyInfo =
                resourceType
                    .GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public).SingleOrDefault(o => o.PropertyType == typeof(ResourceManager));

            var resourceMgr = (System.Resources.ResourceManager)propertyInfo?.GetValue(null, null);
            return resourceMgr?.GetString(resourceKey) ?? resourceKey;

        }

    }
}