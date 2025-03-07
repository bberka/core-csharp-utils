using MailKit.Net.Smtp;
using MimeKit;
using Serilog;

namespace Email;

public sealed class SmtpEmailService(EmailConfig config) : IEmailService
{
  public async Task<ResultStruct.Result> SendEmailAsync(string to, string subject, string body, bool isHtml = false)
  {
    try
    {
      var client = new SmtpClient();
      await client.ConnectAsync(config.SmtpServer, config.SmtpPort, true);
      await client.AuthenticateAsync(config.Username, config.Password);
      var message = new MimeMessage();
      message.Subject = subject;
      message.Body = new TextPart(isHtml
                                    ? "html"
                                    : "plain")
      {
        Text = body
      };
      message.To.Add(new MailboxAddress(to, to));
      message.From.Add(new MailboxAddress(config.FromName, config.FromEmail));
      await client.SendAsync(message);
      await client.DisconnectAsync(true);
      Log.Information("[SmtpEmailService] Email sent successfully");
      return ResultStruct.Result.Success();
    }
    catch (Exception ex)
    {
      Log.Error(ex, "[SmtpEmailService] Email sending failed");
      return ResultStruct.Result.Error();
    }
  }
}