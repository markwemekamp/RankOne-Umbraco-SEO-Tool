using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RankOne.Interfaces;

namespace RankOne.Helpers
{
    public class ReflectionHelper : IReflectionHelper
    {
        public IEnumerable<Assembly> GetAssemblies()
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            return new List<Assembly> { currentAssembly };
        }

        public IEnumerable<Type> GetTypesWithAttribute(Assembly assembly, Type attributeType)
        {
            return assembly.GetTypes()
                .Where(
                    x => Attribute.IsDefined(x, attributeType));
        }

        public T GetAttributeFromType<T>(Type type) where T : Attribute
        {
            return (T)Attribute.GetCustomAttributes(type).FirstOrDefault(y => y is T);
        }
    }
}
