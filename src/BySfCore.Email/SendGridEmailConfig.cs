namespace BySfCore.Email;

public sealed class SendGridEmailConfig
{
  public required string FromEmail { get; set; }

  public required string FromName { get; set; }

  public required string SendGridEmailApiKey { get; set; }
}