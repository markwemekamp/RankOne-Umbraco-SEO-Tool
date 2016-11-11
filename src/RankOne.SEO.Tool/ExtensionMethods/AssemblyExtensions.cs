using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RankOne.ExtensionMethods
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<Type> GetTypesWithAttribute(this Assembly assembly, Type attributeType)
        {
            return assembly.GetTypes()
                .Where(
                    x => Attribute.IsDefined(x, attributeType));
        }
    }
}
