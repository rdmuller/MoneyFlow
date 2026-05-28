using System.Text.Json.Serialization;
using SharedKernel.Abstractions;

namespace SharedKernel.Communications;

public class BaseQueryResponse<T> : BaseResponseGeneric<T>
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("total_pages")]
    public long? TotalPages { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("total_rows")]
    public long? TotalRows { get; set; }
}
