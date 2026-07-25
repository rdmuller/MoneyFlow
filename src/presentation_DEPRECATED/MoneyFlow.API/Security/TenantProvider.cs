using MoneyFlow.Domain.General.Security;
using MoneyFlow.Domain.Tenant.Services;

namespace MoneyFlow.API.Security;

internal class TenantProvider : ITenantProvider
{
    private long TenantId { get; set; }

    long ITenantProvider.Get() => TenantId;

    void ITenantProvider.Set(long tenantId) => TenantId = tenantId;
}
