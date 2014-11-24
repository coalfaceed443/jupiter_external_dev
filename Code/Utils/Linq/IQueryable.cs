using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System.Linq
{
    public static class Paging
    {
        public static IQueryable<TSource> Page<TSource>(this IQueryable<TSource> source, int currentPage, int pageSize)
        {
            return source.Skip((currentPage - 1) * pageSize).Take(pageSize);
        }

        public static int PageCount<TSource>(this IQueryable<TSource> source, int pageSize)
        {
            return (int)Math.Ceiling((decimal)source.Count() / (decimal)pageSize);
        }
    }
}