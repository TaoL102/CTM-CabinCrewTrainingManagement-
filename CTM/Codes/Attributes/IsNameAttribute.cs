using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTM.Codes.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class IsNameAttribute : Attribute
    {

    }
}