using Serilog;

namespace Email;

/// <summary>
/// This is a mock service for smtp service.
///
/// It is a singleton service that is used only in development environment.
/// </summary>
public sealed class MockSmtpEmailService : IEmailService
{
  public async Task<ResultStruct.Result> SendEmailAsync(string to, string subject, string body, bool isHtml = false)
  {
    Log.Information("MockSmtpEmailService: E-posta gönderildi. To:{To}, Subject:{Subject}, Body:{Body}, IsHtml:{IsHtml}", to, subject, body, isHtml);
    return ResultStruct.Result.Success();
  }
}