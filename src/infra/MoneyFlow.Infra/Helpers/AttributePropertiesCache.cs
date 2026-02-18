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
        if (string.IsNullOrWhiteSpace(key) || type == null)
            return null;

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

/*
 
 What changed (short)
•	GetAttributeProperties(Type, string) now accepts dotted paths like "category.name" and will traverse navigation property types to return the nested attribute.
•	Added GetNavigationProperties(Type, string) to return all properties of a navigation property type (useful to list Category attributes when given Sector).
Usage examples
•	Get single nested attribute:
•	GetAttributeProperties(typeof(Sector), "category.name") -> returns AttributeProperties for Name.
•	Get all Category attributes from Sector:
•	var props = GetNavigationProperties(typeof(Sector), "category"); -> dictionary of Category properties keyed by lowercase name.
This lets your code request attributes of a navigation property (Category) when you only have the root type (Sector).


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
        // Support nested navigation paths like "category.name" by traversing types.
        if (string.IsNullOrWhiteSpace(key) || type is null)
            return null;

        var parts = key.Split('.', StringSplitOptions.RemoveEmptyEntries);
        Type currentType = type;
        AttributeProperties? found = null;

        foreach (var part in parts)
        {
            var map = _propertyCache.GetOrAdd(currentType, t =>
                t.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                 .ToDictionary(p => p.Name.ToLower(), p => new AttributeProperties(
                     p.Name,
                     p.PropertyType.ToString().ToLower(),
                     p.PropertyType))
            );

            map.TryGetValue(part.ToLower(), out found);

            if (found is null)
                return null; // path broken

            // if there are more parts, step into the navigation property's type
            currentType = found.RealType;
        }

        return found;
    }

    // Helper to get all properties of a navigation property (e.g. all attributes of Category from Sector)
    public static IReadOnlyDictionary<string, AttributeProperties>? GetNavigationProperties(Type rootType, string navigationPropertyName)
    {
        if (string.IsNullOrWhiteSpace(navigationPropertyName) || rootType is null)
            return null;

        var rootMap = _propertyCache.GetOrAdd(rootType, t =>
            t.GetProperties(BindingFlags.Public | BindingFlags.Instance)
             .ToDictionary(p => p.Name.ToLower(), p => new AttributeProperties(
                 p.Name,
                 p.PropertyType.ToString().ToLower(),
                 p.PropertyType))
        );

        if (!rootMap.TryGetValue(navigationPropertyName.ToLower(), out var navProp))
            return null;

        var navType = navProp.RealType;
        var navMap = _propertyCache.GetOrAdd(navType, t =>
            t.GetProperties(BindingFlags.Public | BindingFlags.Instance)
             .ToDictionary(p => p.Name.ToLower(), p => new AttributeProperties(
                 p.Name,
                 p.PropertyType.ToString().ToLower(),
                 p.PropertyType))
        );

        return navMap;
    }
}


 */