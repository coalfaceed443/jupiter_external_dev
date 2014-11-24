using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Code.Abstracts
{
    public abstract class absShallowCopy<T>
    {
        public object ShallowCopy()
        {
            return (T)this.MemberwiseClone();
        }
    }
}