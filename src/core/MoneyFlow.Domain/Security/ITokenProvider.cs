namespace MoneyFlow.Domain.Security;

public interface ITokenProvider
{
    string TokenOnRequest();
}
