﻿using SharedKernel.Entities;

namespace MoneyFlow.Domain.Tenant.Entities.Wallets;
public class Wallet : BaseEntityTentant
{
    public string Name { get; set; } = string.Empty;
}
