using Shared.Application.Messaging;
using Shared.Domain;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MoneyFlow.Application.UseCases.General.Users.Commands.ChangePassword;

public class UserChangePasswordCommand : IRequest<Result>
{
    [JsonPropertyName("old_password")]
    [Required]
    public string? OldPassword { get; set; } = string.Empty;

    [JsonPropertyName("new_password")]
    [Required]
    public string? NewPassword { get; set; } = string.Empty;
}
