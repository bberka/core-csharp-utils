using System.ComponentModel.DataAnnotations;

namespace Email;

public sealed class EmailConfig
{
  public required string SmtpServer { get; set; }

  [Range(0, 65535)]
  public required ushort SmtpPort { get; set; }

  public bool TLS { get; set; }

  public bool SSL { get; set; }
  public required string Username { get; set; }
  public required string Password { get; set; }
  public required string FromEmail { get; set; }
  public required string FromName { get; set; }
}