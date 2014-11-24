using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Code.Models
{
    /// <summary>
    /// Summary description for Setting
    /// </summary>
    public partial class Setting
    {
        private static MainDataContext db { get { return MainDataContext.CurrentContext; } }

        public static KeyValuePair<string, string> GetSetting(string name)
        {
            Setting setting = db.Settings.FirstOrDefault(a => a.Name == name);

            if (setting == null)
                return new KeyValuePair<string, string>();

            return new KeyValuePair<string, string>(setting.Name, setting.Value);
        }

        public static void SetSetting(string name, object value)
        {
            Setting setting = db.Settings.FirstOrDefault(a => a.Name == name);

            if (setting == null)
            {
                setting = new Setting()
                {
                    Name = name
                };
                db.Settings.InsertOnSubmit(setting);
            }
            setting.Value = value.ToString();

            db.SubmitChanges();
        }

        public static double GetVATRate()
        {
            return double.Parse(GetSetting("VAT").Value);
        }

        public static void SetVATRate(double rate)
        {
            SetSetting("VAT", rate);
        }
    }
}