using System.Text.Json.Serialization;

namespace Shared.Presentation.Communications;

public class BaseQueryResponse<T> : BaseResponse<T>
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("total_pages")]
    public long? TotalPages { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("total_rows")]
    public long? TotalRows { get; set; }
}
