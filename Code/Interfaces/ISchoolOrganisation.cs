using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Models;

namespace CRM.Code.Interfaces
{
    public interface ISchoolOrganisation : IAutocomplete
    {
        CRM_Address CRM_Address { get; set; }
    }
}