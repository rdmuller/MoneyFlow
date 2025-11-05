using System;
using System.ComponentModel;

namespace SharedKernel.Communications;
public class QueryParams
{
    public long? PageNum { get; set; }
    public long? PageRows { get; set; }

    [Description("\"F\" - Full, \"A\" - Active, \"I\" - Inactive")]
    public string? Status { get; set; }

    public Dictionary<string, string>? ExtraParams { get; set; }
}

public enum StatusFilter
{
    Full = 0,
    Active = 1,
    Inactive = 2
}

public static class StatusFilterExtensions
{
    private static readonly Dictionary<StatusFilter, string> _statusFilterMappings = new()
    {
        { StatusFilter.Full, "F" },
        { StatusFilter.Active, "A" },
        { StatusFilter.Inactive, "I" }
    };

    public static StatusFilter FromCode(string? code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return StatusFilter.Active;

        var statusFilter = _statusFilterMappings.FirstOrDefault(x => x.Value.Equals(code, StringComparison.OrdinalIgnoreCase));
        if (statusFilter.Key != null)
            return statusFilter.Key;

        return StatusFilter.Active; // Default value
    }
}