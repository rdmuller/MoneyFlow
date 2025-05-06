﻿namespace MoneyFlow.Domain.Security;

public interface IPasswordHasher
{
    string Hash(string password);

    bool Verify(string password, string hashedPassword);
}
