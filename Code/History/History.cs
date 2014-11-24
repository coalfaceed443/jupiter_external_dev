using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Reflection;
using System.Data.Linq.Mapping;
using System.Security.Principal;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections;
using CRM.Code.Models;
using CRM.Code.Interfaces;
using Newtonsoft.Json;

namespace CRM.Code.Models
{
    
    public partial class MainDataContext
{

    [Function(Name = "GetDate", IsComposable = true)]
    public DateTime GetSystemDate()
    {
        MethodInfo mi = MethodBase.GetCurrentMethod() as MethodInfo;
        return (DateTime)this.ExecuteMethodCall(this, mi, new object[] { }).ReturnValue;
    }
}

}

namespace CRM.Code.History
{


 
        public class History
        {
            protected class IsPrimaryKeyAttribute : Attribute
            {
                public IsPrimaryKeyAttribute(bool isPrimaryKey)
                {
                    this.isPrimaryKey = isPrimaryKey;
                }
                protected bool isPrimaryKey;
                public bool IsPrimaryKey
                {
                    get
                    {
                        return this.isPrimaryKey;
                    }
                }
            }

            public class HistoryException : Exception
            {
                public HistoryException(string message)
                    : base("History recording error: " + message)
                {
                }
            }

            protected const int KeyMax = 1;

            protected static bool AttrIsPrimaryKey(PropertyInfo pi)
            {
                var attrs =
                    from attr in pi.GetCustomAttributes(typeof(ColumnAttribute), true)
                    where ((ColumnAttribute)attr).IsPrimaryKey
                    select attr;

                if (attrs != null && attrs.Count() > 0)
                    return true;
                else
                    return false;
            }

            protected static bool AttrIsForeignKey(PropertyInfo pi)
            {
                var attrs =
                    from attr in pi.GetCustomAttributes(typeof(AssociationAttribute), true)
                    where ((AssociationAttribute)attr).IsForeignKey
                    select attr;

                if (attrs != null && attrs.Count() > 0)
                    return true;
                else
                    return false;
            }


            public static void RecordLinqInsert(Models.Admin Admin, object obj)
            {
                MainDataContext dbo = new MainDataContext();
                RecordLinqInsert(dbo, Admin, obj);
                dbo.SubmitChanges();
            }
            public static void RecordLinqInsert(MainDataContext dbo, Models.Admin Admin, object obj)
            {
                TableHistory hist = NewHistoryRecord(obj);

                hist.ActionType = "INSERT";
                hist.ActionUserName = Admin.DisplayName + " " + Admin.ID.ToString();
                hist.ActionDateTime = dbo.GetSystemDate();
                hist.ParentID = ((IHistory)obj).ParentID.ToString();
                                hist.OldValue = SimpleSerialize(obj);
                dbo.TableHistories.InsertOnSubmit(hist);
            }

            public static void RecordLinqInsertAll(IIdentity user, IEnumerable ie)
            {
                MainDataContext dbo = new MainDataContext();
                RecordLinqInsertAll(dbo, user, ie);
                dbo.SubmitChanges();
            }
            public static void RecordLinqInsertAll(MainDataContext dbo, IIdentity user, IEnumerable ie)
            {
                List<TableHistory> histList = new List<TableHistory>();
                string userName = user.Name;
                DateTime systemDate = dbo.GetSystemDate();

                foreach (object obj in ie)
                {
                    TableHistory hist = NewHistoryRecord(obj);
                    hist.ActionType = "INSERT";
                    hist.ActionUserName = userName;
                    hist.ActionDateTime = systemDate;
                    histList.Add(hist);
                }

                dbo.TableHistories.InsertAllOnSubmit(histList);
            }

            public static void RecordLinqDelete(Models.Admin user, object obj)
            {
                MainDataContext dbo = new MainDataContext();
                TableHistory hist = NewHistoryRecord(obj);
                
                hist.ActionType = "DELETE";
                hist.ActionUserName = user.LoggerName;
                hist.ActionDateTime = dbo.GetSystemDate();

                hist.OldValue = SimpleSerialize(obj);

                dbo.TableHistories.InsertOnSubmit(hist);
                dbo.SubmitChanges();
            }

            public static string SimpleSerialize(object obj)
            {
                Type type = obj.GetType();
                var props = from pi in type.GetProperties()
                            where pi.PropertyType.Namespace == "System"
                                select pi;


                List<Dictionary<string, string>> storeProp = new List<Dictionary<string,string>>();
                foreach (PropertyInfo pi in props)
                {
                    Dictionary<string, string> item = new Dictionary<string, string>();
                    string value = pi.GetValue(obj, null) == null ? "NULL" : TruncateCharactersPastLimit(pi.GetValue(obj, null).ToString());
                    item.Add(pi.Name, value);
                    storeProp.Add(item);
                }

                return JsonConvert.SerializeObject(storeProp);

            }

            public static void RecordLinqDeleteAll(Models.Admin user, IEnumerable ie)
            {
                MainDataContext dbo = new MainDataContext();
                RecordLinqDeleteAll(dbo, user, ie);
                dbo.SubmitChanges();
            }
            public static void RecordLinqDeleteAll(MainDataContext dbo, Models.Admin user, IEnumerable ie)
            {
                List<TableHistory> histList = new List<TableHistory>();
                string userName = user.LoggerName;
                DateTime systemDate = dbo.GetSystemDate();

                foreach (object obj in ie)
                {
                    TableHistory hist = NewHistoryRecord(obj);
                    hist.ActionType = "DELETE";
                    hist.ActionUserName = user.LoggerName;
                    hist.ActionDateTime = systemDate;
                    hist.ParentID = ((IHistory)obj).ParentID.ToString();
                    histList.Add(hist);
                }

                dbo.TableHistories.InsertAllOnSubmit(histList);
            }

            public static void RecordLinqUpdate(Models.Admin user, object oldObj, object newObj)
            {
                using (MainDataContext dbo = new MainDataContext())
                {
                    RecordLinqUpdate(dbo, user, oldObj, newObj);
                    dbo.SubmitChanges();
                }
            }

            public static void RecordLinqUpdate(MainDataContext dbo, Models.Admin user, object oldObj, object newObj)
            {
                List<TableHistory> histList = GetLinqUpdateChangeList(dbo, user, oldObj, newObj);
                dbo.TableHistories.InsertAllOnSubmit(histList);
                dbo.SubmitChanges();
            }

            public static List<TableHistory> GetLinqUpdateChangeList(Models.Admin user, object oldObj, object newObj)
            {
                using (MainDataContext dbo = new MainDataContext())
                {
                    return GetLinqUpdateChangeList(dbo, user, oldObj, newObj);
                }
            }
            public static List<TableHistory> GetLinqUpdateChangeList(MainDataContext dbo, Models.Admin user, object oldObj, object newObj)
            {
                List<TableHistory> histList = new List<TableHistory>();

                Type type = oldObj.GetType();
                if (newObj.GetType() != type)
                    throw new HistoryException("type mismatch between old object and new object.");

                var changed =
                    from pi in type.GetProperties()
                    where pi.PropertyType.Namespace == "System" &&
                        String.Format("{0}", pi.GetValue(oldObj, null)) != String.Format("{0}", pi.GetValue(newObj, null))
                    select pi;

                foreach (PropertyInfo pi in changed)
                {
                    TableHistory hist = NewHistoryRecord(oldObj);
                    hist.ActionType = "UPDATE";
                    hist.Property = pi.Name;
                    hist.OldValue = pi.GetValue(oldObj, null) == null ? "NULL" : TruncateCharactersPastLimit(pi.GetValue(oldObj, null).ToString());
                    hist.NewValue = pi.GetValue(newObj, null) == null ? "NULL" : TruncateCharactersPastLimit(pi.GetValue(newObj, null).ToString());
                    hist.ActionUserName = user.LoggerName;
                    hist.ActionDateTime = dbo.GetSystemDate();
                    hist.ParentID = ((IHistory)newObj).ParentID.ToString();
                    histList.Add(hist);
                }

                return histList;
            }

            public static void RecordSqlUpdate(Models.Admin user, string tableName, IOrderedDictionary keys, IOrderedDictionary oldObj, IOrderedDictionary newObj)
            {
                MainDataContext dbo = new MainDataContext();
                List<TableHistory> histList = new List<TableHistory>();

                foreach (DictionaryEntry entry in oldObj)
                {
                    if (String.Format("{0}", entry.Value) == String.Format("{0}", newObj[entry.Key]))
                        continue;

                    TableHistory hist = NewHistoryRecord(tableName, keys);
                    hist.ActionType = "UPDATE";
                    hist.Property = entry.Key.ToString();
                    hist.OldValue = TruncateCharactersPastLimit(String.Format("{0}", entry.Value));
                    hist.NewValue = TruncateCharactersPastLimit(String.Format("{0}", newObj[entry.Key]));
                    hist.ActionUserName = user.LoggerName;
                    hist.ActionDateTime = dbo.GetSystemDate();
                    histList.Add(hist);
                }
                dbo.TableHistories.InsertAllOnSubmit(histList);
                dbo.SubmitChanges();
            }

            private static TableHistory NewHistoryRecord(string tableName, IOrderedDictionary keys)
            {
                TableHistory hist = new TableHistory();

                if (keys.Count > KeyMax)
                    throw new HistoryException("object has more than " + KeyMax.ToString() + " keys.");
                for (int i = 1; i <= keys.Count; i++)
                {
                    typeof(TableHistory)
                        .GetProperty("Key" + i.ToString())
                        .SetValue(hist, keys[i - 1].ToString(), null);
                }
                hist.TableName = tableName;

                return hist;
            }

            private static Dictionary<Type, List<PropertyInfo>> historyRecordExceptions = new Dictionary<Type, List<PropertyInfo>>();

            private static TableHistory NewHistoryRecord(object obj)
            {
                TableHistory hist = new TableHistory();

                Type type = obj.GetType();
                PropertyInfo[] keys;
      
                    keys = type.GetProperties().Where(o => AttrIsPrimaryKey(o)).ToArray();
                

                if (keys.Length > KeyMax)
                    throw new HistoryException("object has more than " + KeyMax.ToString() + " keys.");
                for (int i = 1; i <= keys.Length; i++)
                {
                    typeof(TableHistory)
                        .GetProperty("Key" + i.ToString())
                        .SetValue(hist, keys[i - 1].GetValue(obj, null).ToString(), null);
                }
                hist.TableName = type.Name;
                hist.ParentID = ((IHistory)obj).ParentID.ToString();
                return hist;
            }

            private static string TruncateCharactersPastLimit(string str)
            {
                const int limit = 8000;
                if (str.Length > limit)
                    return str.Substring(0, limit);
                return str;
            }
        }
    }