using System.Collections.Concurrent;
using System.Reflection;

namespace MoneyFlow.Infra.Helpers;
internal static class PropertyCache
{
    private static readonly ConcurrentDictionary<Type, Dictionary<string, string>> _propertyCache = new();

    public static string? GetRealPropertyName(Type type, string key)
    {
        var map = _propertyCache.GetOrAdd(type, t => 
            t.GetProperties(BindingFlags.Public | BindingFlags.Instance)
             .ToDictionary(p => p.Name.ToLower(), p => p.Name)
        );

        map.TryGetValue(key.ToLower(), out var realName);

        return realName ?? null;
    }
}
