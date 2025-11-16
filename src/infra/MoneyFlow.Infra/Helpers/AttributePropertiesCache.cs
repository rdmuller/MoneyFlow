using System.Collections.Concurrent;
using System.Reflection;

namespace MoneyFlow.Infra.Helpers;

internal static class AttributePropertiesCache
{
    /// <summary>
    /// Type that represents the real name and type of a property.
    /// Type is in lower case.
    /// </summary>
    /// <param name="RealName"></param>
    /// <param name="Type"></param>
    internal record AttributeProperties(string RealName, string Type, Type RealType);

    private static readonly ConcurrentDictionary<Type, Dictionary<string, AttributeProperties>> _propertyCache = new();

    public static AttributeProperties? GetAttributeProperties(Type type, string key)
    {
        var map = _propertyCache.GetOrAdd(type, t =>
            t.GetProperties(BindingFlags.Public | BindingFlags.Instance)
             .ToDictionary(p => p.Name.ToLower(), p => new AttributeProperties(
                 p.Name, 
                 p.PropertyType.ToString().ToLower(),
                 p.PropertyType))
        );

        map.TryGetValue(key.ToLower(), out var attributeProperties);

        return attributeProperties ?? null;
    }
}