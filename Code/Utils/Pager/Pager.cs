using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace CRM.Code.Utils.Pager
{
    /// <summary>
    /// Simple paging utility class for collections
    /// </summary>
    public class Pager<T>
    {
        private IEnumerable<T> items;
        private int count;
        private int itemsPerPage;
        private int pages;

        public Pager(IEnumerable<T> items, int itemsPerPage)
        {
            this.items = items;
            this.count = this.items.Count();
            this.itemsPerPage = itemsPerPage;

            this.pages = ((this.count - 1) / this.itemsPerPage) + 1;
        }

        public int Pages
        {
            get
            {
                return pages;
            }
        }

        public int Count
        {
            get
            {
                return count;
            }
        }

        public IEnumerable<T> GetItems(int page)
        {
            return this.items.Skip(itemsPerPage * (page - 1)).Take(itemsPerPage);
        }
    }
}