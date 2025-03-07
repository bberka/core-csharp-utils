using SendGrid;
using SendGrid.Helpers.Mail;
using Serilog;

namespace Email;

public sealed class SendGridEmailConfig
{
  public required string FromEmail { get; set; }

  public required string FromName { get; set; }

  public required string SendGridEmailApiKey { get; set; }
}

public sealed class SendGridEmailService(SendGridEmailConfig config) : IEmailService
{
  public async Task<Result> SendEmailAsync(string toEmail, string mailSubject, string body, bool isHtml = false)
  {
    var client = new SendGridClient(config.SendGridEmailApiKey);
    var from = new EmailAddress(config.FromEmail, config.FromName);
    var to = new EmailAddress(toEmail);
    var msg = MailHelper.CreateSingleEmail(from,
                                           to,
                                           mailSubject,
                                           isHtml
                                             ? ""
                                             : body,
                                           isHtml
                                             ? body
                                             : "");
    var response = await client.SendEmailAsync(msg);
    var responseString = await response.Body.ReadAsStringAsync();
    var isSuccess = response.IsSuccessStatusCode;
    if (isSuccess)
    {
      Log.Information("[SendGridEmailService] Email sent successfully. Response: {Response}", responseString);
      return Result.Success();
    }
    else
    {
      Log.Error("[SendGridEmailService] Email sending failed. Response: {Response}", responseString);
      return Result.Error();
    }
  }
}