using Microsoft.Extensions.Configuration;

namespace MoneyFlow.Infra.Settings;

internal class EmailSettings
{
    public string SmtpServer { get; set; } = string.Empty;
    public int SmtpPort { get; set; } = 587;
    public string SmtpUser { get; set; } = string.Empty;
    public bool EnableSsl { get; set; }
    public string SmtpPassword { get; set; } = string.Empty;
    public string FromEmail { get; set; } = string.Empty;
    public string FromName { get; set; } = "MoneyFlow";

    public static EmailSettings GetSettings(IConfiguration config)
    {
        var emailSettings = config.GetSection("Settings:EmailSettings").Get<EmailSettings>() ?? new EmailSettings();

        if (string.IsNullOrWhiteSpace(emailSettings.FromEmail))
        {
            emailSettings.SmtpServer = Environment.GetEnvironmentVariable("MONEYFLOW_EMAIL_SMTP_SERVER") ?? "";
            emailSettings.SmtpPort = int.Parse(Environment.GetEnvironmentVariable("MONEYFLOW_EMAIL_SMTP_PORT"));
            emailSettings.SmtpUser = Environment.GetEnvironmentVariable("MONEYFLOW_EMAIL_SMTP_USER") ?? "";
            emailSettings.SmtpPassword = Environment.GetEnvironmentVariable("MONEYFLOW_EMAIL_SMTP_PASSWORD") ?? "";
            emailSettings.EnableSsl = bool.Parse(Environment.GetEnvironmentVariable("MONEYFLOW_EMAIL_SSL"));

            emailSettings.FromEmail = Environment.GetEnvironmentVariable("MONEYFLOW_EMAIL_FROM_EMAIL") ?? "";
            emailSettings.FromName = Environment.GetEnvironmentVariable("MONEYFLOW_EMAIL_FROM_NAME") ?? "";
        }

        return emailSettings;
    }
}