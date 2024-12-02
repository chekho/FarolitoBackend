using SendGrid;
using SendGrid.Helpers.Mail;

namespace FarolitoAPIs.Services;

public interface IEmailService
{
    Task EnviarCorreoAsync(string toEmail, string subject, string mensaje);
}

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task EnviarCorreoAsync(string toEmail, string subject, string mensaje)
    {
        var apiKey = _configuration["MyVars:ApiUrl"];
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress("sergiocecyteg@gmail.com", "Farolito");
        var to = new EmailAddress(toEmail);
        var msg = MailHelper.CreateSingleEmail(from, to, subject, mensaje, string.Empty);

        await client.SendEmailAsync(msg);
    }
}