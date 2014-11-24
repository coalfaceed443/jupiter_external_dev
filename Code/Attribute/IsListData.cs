using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;

[System.AttributeUsage(System.AttributeTargets.Property)
]
public class IsListData : System.Attribute
{
    public bool UseInList;
    public string FriendlyName;

    public IsListData(bool isListData, string friendlyName) : this(friendlyName)
    {
        UseInList = isListData;
        FriendlyName = friendlyName;
    }

    public IsListData(string friendlyName)
    {
        UseInList = true;
        FriendlyName = friendlyName;
    }


    public static IsListData GetTypeWithAttribute(Type t, string propertyName)
    {
        var pi = t.GetProperty(propertyName);
        var attr = (IsListData[])pi.GetCustomAttributes(typeof(IsListData), false);
        return attr.FirstOrDefault(f => f.UseInList);
    }

}


