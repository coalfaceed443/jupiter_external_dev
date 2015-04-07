using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Code.Models
{
    public partial class CRM_PersonRelationship
    {

        public CRM_RelationCode PersonACode
        {
            get
            {
                return this.CRM_RelationCode;
            }
        }

        public CRM_RelationCode PersonBCode
        {

            get
            {
                return this.CRM_RelationCode1;
            }
        }

        public CRM_Person PersonA
        {
            get
            {
                return this.CRM_Person;
            }
        }

        public CRM_Person PersonB
        {
            get
            {
                return this.CRM_Person1;
            }
        }

        public CRM_Address Address
        {
            get
            {
                return this.CRM_Person2.CRM_Address;
            }
        }

    }
}