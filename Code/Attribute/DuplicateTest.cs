using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;

[System.AttributeUsage(System.AttributeTargets.Property)]
public class DuplicateTest : System.Attribute
{
    public static DuplicateTest GetTypeWithAttribute(Type t, string propertyName)
    {
        var pi = t.GetProperty(propertyName);
        var attr = (DuplicateTest[])pi.GetCustomAttributes(typeof(DuplicateTest), false);
        return attr.FirstOrDefault();
    }
}
