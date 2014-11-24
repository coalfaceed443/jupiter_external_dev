using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using CRM.Code.Models;
using CRM.Code.Interfaces;

namespace CRM.Code.Abstracts
{
    public abstract class absArchivable
    {
        public static IEnumerable<ListItem> SetDropDown(IEnumerable<IArchivable> baseSet, IArchivable Current)
        {
            return from c in baseSet
                    where (c.IsActive && !c.IsArchived) || (Current != null && c.ID == Current.ID)
                    select c.ListItem;
          
        }

        public static IEnumerable<ListItem> SetDropDownWithString(IEnumerable<IArchivable> baseSet, string Current)
        {
            return from c in baseSet
                   where (c.IsActive && !c.IsArchived) || (!String.IsNullOrEmpty(Current) && c.ListItem.Text == Current)
                   select c.ListItem;

        }

        public static IEnumerable<ListItem> SetCheckboxList(IEnumerable<IArchivable> baseSet, IEnumerable<ICRMRecord> Current)
        {
            return from c in baseSet
                   where (c.IsActive && !c.IsArchived) || (Current != null && Current.Any(cu => c.ID == cu.ID))
                   select c.ListItem;

        }

    }
}