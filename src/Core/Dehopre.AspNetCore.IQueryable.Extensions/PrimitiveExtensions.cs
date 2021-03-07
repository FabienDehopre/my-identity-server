namespace Dehopre.AspNetCore.IQueryable.Extensions
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    internal static class PrimitiveExtensions
    {
        private static readonly ConcurrentDictionary<Type, List<PropertyInfo>> MemoryObjects;

        static PrimitiveExtensions() => MemoryObjects = new();

        public static bool IsPropertyCollection(this PropertyInfo property) => IsGenericEnumerable(property.PropertyType) || property.PropertyType.IsArray;

        public static bool IsPropertObject(this PropertyInfo property, object value) => Convert.GetTypeCode(property.GetValue(value, null)) == TypeCode.Object;

        private static bool IsGenericEnumerable(Type type)
            => type.IsGenericType &&
               type.GetInterfaces().Any(ti => (ti == typeof(IEnumerable<>) || ti.Name == "IEnumerable"));

        internal static List<PropertyInfo> GetAllProperties(this Type type)
        {
            if (MemoryObjects.ContainsKey(type))
            {
                return MemoryObjects[type];
            }

            var properties = type.GetProperties().ToList();
            MemoryObjects.TryAdd(type, properties);
            return properties;
        }

        internal static PropertyInfo GetProperty(Type type, string name)
            => type.GetAllProperties().FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        internal static PropertyInfo GetProperty<TEntity>(string name)
            => typeof(TEntity).GetAllProperties().FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        public static bool HasProperty(this Type type, string propertyName)
            => type.GetAllProperties().Any(p => p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));

        public static string[] Fields(this string fields)
            => fields.Split(',');

        public static string FieldName(this string field)
            => field.StartsWith("-", "+") ? field[1..].Trim() : field.Trim();

        public static bool StartsWith(this string text, params string[] with)
            => with.Any(text.StartsWith);

        public static bool IsDescending(this string field)
            => field.StartsWith("-");
    }
}
