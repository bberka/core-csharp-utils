namespace BySfCore.Email;

public interface IEmailService
{
  Task<ResultStruct.Result> SendEmailAsync(string to, string subject, string body, bool isHtml = false);
}