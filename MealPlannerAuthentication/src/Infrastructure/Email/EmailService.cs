using System.Net.Mail;
using invensys.iserve.Application.Common.Interfaces;
using invensys.iserve.Infrastructure.Config;
using Microsoft.Extensions.Configuration;

namespace invensys.iserve.Infrastructure.Email;
public class EmailService(SmtpClient smtpClient, IConfiguration configuration) : IEmailService
{
   public async Task SendEmailAsync(string email, string subject, string body)
   {
      var emailConfiguration = configuration.GetSection(nameof(EmailConfiguration)).Get<EmailConfiguration>();
      Guard.Against.Null(emailConfiguration, nameof(EmailConfiguration));
      Guard.Against.NullOrEmpty(email);

      var mailMessage = new MailMessage
      {
         From = new MailAddress(emailConfiguration.SmtpUsername),
         Subject = subject,
         Body = body,
         IsBodyHtml = true,
         To = { new MailAddress(email) }
      };

      await smtpClient.SendMailAsync(mailMessage);
   }
}
