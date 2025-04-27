using System.Text.Json.Serialization;

namespace MoneyFlow.Common.Communications;

public class BaseRequest<T>
{
    [JsonPropertyName("name")]
    public T? Data { get; set; }
}