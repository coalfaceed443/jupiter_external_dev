using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Models;

namespace CRM.Code.Interfaces
{
    public interface IContact : IAutocomplete, IMailable
    {
        int AddressID { get; }
        string Title { get; }
        string Fullname { get; }
        string Firstname { get; }
        string Lastname { get; }
        CRM_Address PrimaryAddress { get; }
        string DisplayName { get; }
        string Reference { get; }
        List<string> Tokens { get; }
        string OutputTableName { get; }
        string CRM_PersonReference { get; }
        CRM_Person Parent_CRM_Person { get; }
        string Photo { get; }
        bool IsArchived { get; }
        int? RelationshipID { get; }
    }
}