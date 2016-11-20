using System;
using System.Linq;

namespace RankOne.ExtensionMethods
{
    public static class TypeExtensions
    {
        public static T GetAttributeWithType<T>(this Type type) where T : Attribute
        {
            return (T)Attribute.GetCustomAttributes(type).FirstOrDefault(y => y is T);
        }

        public static T GetInstance<T>(this Type type)
        {
            var instance = Activator.CreateInstance(type);
            return (T)instance;
        }
    }
}
