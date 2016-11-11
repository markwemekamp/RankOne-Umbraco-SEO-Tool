using System;
using System.Collections.Generic;
using System.Reflection;

namespace RankOne.Interfaces
{
    public interface IReflectionHelper
    {
        IEnumerable<Assembly> GetAssemblies();
        IEnumerable<Type> GetTypesWithAttribute(Assembly assembly, Type attributeType);
        T GetAttributeFromType<T>(Type type) where T : Attribute;
    }
}
