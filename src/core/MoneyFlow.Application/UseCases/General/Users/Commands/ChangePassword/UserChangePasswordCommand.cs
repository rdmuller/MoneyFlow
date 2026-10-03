using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Shared.Application.Messaging;

namespace MoneyFlow.Application.UseCases.General.Users.Commands.ChangePassword;

public sealed class UserChangePasswordCommand : ICommand
{
    [JsonPropertyName("old_password")]
    [Required(ErrorMessage = "Old password is required")]
    public string? OldPassword { get; set; } = string.Empty;

    [JsonPropertyName("new_password")]
    [Required(ErrorMessage = "New password is required")]
    public string? NewPassword { get; set; } = string.Empty;
}
