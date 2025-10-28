using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SharedKernel.Communications;

public class BaseRequest<T>
{
    [JsonPropertyName("data")]
    //[Required(ErrorMessage = "Tag 'data' deve ser informada")]
    public T? Data { get; set; }
}