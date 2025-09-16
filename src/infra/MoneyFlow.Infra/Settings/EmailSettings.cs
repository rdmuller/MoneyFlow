using Microsoft.Extensions.Configuration;

namespace MoneyFlow.Infra.Settings;

internal class EmailSettings
{
    public string SmtpServer { get; private set; } = string.Empty;
    public int SmtpPort { get; private set; } = 587;
    public string SmtpUser { get; private set; } = string.Empty;
    public bool EnableSsl { get; private set; }
    public string SmtpPassword { get; private set; } = string.Empty;
    public string FromEmail { get; private set; } = string.Empty;
    public string FromName { get; private set; } = "MoneyFlow";

    public static EmailSettings GetSettings(IConfiguration config)
    {
        var emailSettings = config.GetSection("Settings:EmailSettings").Get<EmailSettings>() ?? new EmailSettings();

        if (string.IsNullOrWhiteSpace(emailSettings.FromEmail))
        {
            emailSettings.SmtpServer = Environment.GetEnvironmentVariable("MONEYFLOW_EMAIL_SMTP_SERVER") ?? "";

            var smtpPortEnv = Environment.GetEnvironmentVariable("MONEYFLOW_EMAIL_SMTP_PORT");
            emailSettings.SmtpPort = int.TryParse(smtpPortEnv, out var port) ? port : 587;

            emailSettings.SmtpUser = Environment.GetEnvironmentVariable("MONEYFLOW_EMAIL_SMTP_USER") ?? "";
            emailSettings.SmtpPassword = Environment.GetEnvironmentVariable("MONEYFLOW_EMAIL_SMTP_PASSWORD") ?? "";

            var enableSslEnv = Environment.GetEnvironmentVariable("MONEYFLOW_EMAIL_SSL");
            emailSettings.EnableSsl = bool.TryParse(enableSslEnv, out var enableSsl) && enableSsl;

            emailSettings.FromEmail = Environment.GetEnvironmentVariable("MONEYFLOW_EMAIL_FROM_EMAIL") ?? "";
            emailSettings.FromName = Environment.GetEnvironmentVariable("MONEYFLOW_EMAIL_FROM_NAME") ?? "";
        }

        return emailSettings;
    }
}
/*
SMTP_SERVER: smtp.gmail.com
SMTP_PORT: 587
SMTP_ENABLE_SSL: true
SMTP_SENDER_EMAIL: seu-email @gmail.com
SMTP_SENDER_NAME: Seu Nome (ou outro nome descritivo)
SMTP_USERNAME: seu-email @gmail.com
SMTP_PASSWORD: A senha de aplicativo gerada
*/