﻿namespace MoneyFlow.Application.DTOs.Auth;
public class TokenDTO
{
    public string Token { get; set; } = string.Empty;

    public DateTimeOffset ExpiresAt { get; set; }
}
