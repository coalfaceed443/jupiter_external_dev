using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Models;

namespace CRM.Code.Interfaces
{
    public interface IAddress
    {
        string AddressLine1 { get; set; }
        string AddressLine2 { get; set; }
        string AddressLine3 { get; set; }
        string AddressLine4 { get; set; }
        string AddressLine5 { get; set; }
        string Town { get; set; }
        string County { get; set; }
        string Postcode { get; set; }
        Country Country { get; set; }
        int CountryID { get; set; }
    }
}