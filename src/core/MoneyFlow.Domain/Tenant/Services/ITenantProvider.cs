namespace MoneyFlow.Domain.Tenant.Services;

public interface ITenantProvider
{
    void Set(long tenantId);

    long Get();
}
