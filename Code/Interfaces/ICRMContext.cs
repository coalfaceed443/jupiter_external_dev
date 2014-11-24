using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq;
using System.Reflection;
using CRM.Code.Models;

namespace CRM.Code.Interfaces
{
    public interface ICRMContext
    {
        string DisplayName { get; }        
    }

    public static class ExtensionDataContext
    {
        /// <summary>
        /// Obtain the DataContext providing this entity
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static MainDataContext GetContext(this ICRMContext obj)
        {
            FieldInfo fEvent = obj.GetType().GetField("PropertyChanging", BindingFlags.NonPublic | BindingFlags.Instance);
            MulticastDelegate dEvent = (MulticastDelegate)fEvent.GetValue(obj);
            Delegate[] onChangingHandlers = dEvent.GetInvocationList();

            // Obtain the ChangeTracker
            foreach (Delegate handler in onChangingHandlers)
            {
                if (handler.Target.GetType().Name == "StandardChangeTracker")
                {
                    // Obtain the 'services' private field of the 'tracker'
                    object tracker = handler.Target;
                    object services = tracker.GetType().GetField("services", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(tracker);

                    // Get the Context
                    MainDataContext context = services.GetType().GetProperty("Context").GetValue(services, null) as MainDataContext;
                    return context;
                }
            }

            // Not found
            throw new Exception("Error reflecting object");
        }
    }
}