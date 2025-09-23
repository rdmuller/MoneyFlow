namespace MoneyFlow.Domain.Common.Services.Email;

public class EmailMessage
{
    public IEnumerable<string> To { get; set; } = [];
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}
