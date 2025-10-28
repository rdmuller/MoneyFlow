namespace MoneyFlow.Domain.General.Security;

public interface ITokenProvider
{
    string TokenOnRequest();
}
