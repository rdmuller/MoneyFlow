namespace MoneyFlow.Domain.Common.Security;

public interface ITokenProvider
{
    string TokenOnRequest();
}
