using System;
using System.Collections.Generic;
using System.Linq;

namespace QuickLoad.Drawers.Interface {
    internal static class InterfaceUtility {
        internal static IEnumerable<Type> GetDerivedTypes(Type type, bool withBaseType = true) {
            string[] interfaces = {
                "Assembly-CSharp",
                "Assembly-CSharp-firstpass",
            };
            
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(asm => interfaces.Any(include => asm.FullName.StartsWith(include)))
                .SelectMany(s => s.GetTypes()).Where(t => type.IsAssignableFrom(t) && (withBaseType || t != type));
        }
    }
}