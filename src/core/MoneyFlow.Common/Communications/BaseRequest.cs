using System.Text.Json.Serialization;

namespace MoneyFlow.Common.Communications;

public class BaseRequest<T>
{
    [JsonPropertyName("data")]
    public T? Data { get; set; }
}