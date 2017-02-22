using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Resources;
using CTMLib.Resources;

namespace CTMLib.Helpers
{
    public static class LocalizationHelper
    {
        public static string GetModelString(string name)
        {
            return Resources.ConstModels.ResourceManager.GetString(name) ?? name;
        }

        public static string GetViewString(string name)
        {
            return Resources.ConstViews.ResourceManager.GetString(name) ?? name;
        }

    }
}