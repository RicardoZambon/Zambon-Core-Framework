using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Zambon.Core.Database.Helper
{
    public class AssemblyHelper
    {

        #region Methods

        public static List<I> GetReferencedClasses<I>(Assembly rootAssembly) where I : class
        {
            var list = new List<I>();

            list.AddRange(rootAssembly.GetActualClasses<I>(rootAssembly.GetTypesByInterface<I>()));

            foreach (var assembly in rootAssembly.GetReferencedAssemblies())
            {
                var dll = Assembly.Load(assembly);

                var types = dll.GetTypesByInterface<I>();
                var classes = dll.GetActualClasses<I>(types);
                list.AddRange(classes);

                dll = null;
            }
            return list;
        }

        #endregion

    }
}