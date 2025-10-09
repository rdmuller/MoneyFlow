using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Mediator.Abstractions;
using MoneyFlow.Common.Communications;

namespace MoneyFlow.Application.UseCases.Common.Users.Commands.ChangePassword;

public class UserChangePasswordCommand : IRequest<BaseResponse<string>>
{
    [JsonPropertyName("old_password")]
    [Required]
    public string? OldPassword { get; set; } = string.Empty;

    [JsonPropertyName("new_password")]
    [Required]
    public string? NewPassword { get; set; } = string.Empty;
}
