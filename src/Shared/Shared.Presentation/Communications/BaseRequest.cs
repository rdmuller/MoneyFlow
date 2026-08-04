using System.Text.Json.Serialization;

namespace Shared.Presentation.Communications;

public class BaseRequest<T>
{
    [JsonPropertyName("data")]
    //[Required(ErrorMessage = "Tag 'data' deve ser informada")]
    public T Data { get; set; }
}
