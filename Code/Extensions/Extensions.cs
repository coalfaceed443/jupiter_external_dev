using CRM.Code.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Code.Extensions
{
    public static class Extensions
    {
        public static List<CRM_Person> DistinctOnRelations(this List<CRM_Person> persons)
        {

            IEnumerable<IGrouping<int?, CRM_Person>> groupset = persons.GroupBy(g => g.RelationshipID);

           

            var recordsWithRelation = groupset.Where(c => c.Key != null).Select(c => c.First());
            var recordsWithoutRelation = groupset.Where(c => c.Key == null).SelectMany(c => c);
            var data = recordsWithRelation.Concat(recordsWithoutRelation);


            return data.ToList();
        }


        public static List<CRM_Person> DistinctOnNamePostcode(this List<CRM_Person> persons)
        {
            IEnumerable<IGrouping<string, CRM_Person>> groupset = persons.GroupBy(g => g.Fullname + g.PrimaryAddressPostcode);            

            return groupset.Select(c => c.First()).ToList();
        }

    }



}