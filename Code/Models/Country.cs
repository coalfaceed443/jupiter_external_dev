using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace CRM.Code.Models
{

    public partial class Country : WebCountry
    {
        public WebCountry WebObject
        {
            get
            {
                return (WebCountry)this;
            }
        }
    }

    [Serializable]
    public class WebCountry
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public WebCountry()
        {

        }

        public WebCountry(int id, string name) : this()
        {
            ID = id;
            Name = name;
        }

    }
}