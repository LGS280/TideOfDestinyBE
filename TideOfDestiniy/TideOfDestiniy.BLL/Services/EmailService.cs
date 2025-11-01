using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;
using TideOfDestiniy.BLL.Interfaces;

namespace TideOfDestiniy.BLL.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            var emailSettings = configuration.GetSection("EmailSettings");
            
            _smtpHost = emailSettings["SmtpHost"] ?? "smtp.gmail.com";
            _smtpPort = int.Parse(emailSettings["SmtpPort"] ?? "587");
            _smtpUsername = emailSettings["SmtpUsername"] ?? string.Empty;
            // Remove spaces from app password if any
            _smtpPassword = (emailSettings["SmtpPassword"] ?? string.Empty).Replace(" ", "");
            _fromEmail = emailSettings["FromEmail"] ?? string.Empty;
            _fromName = emailSettings["FromName"] ?? "Tide of Destiny";
            
            // Validate required settings
            if (string.IsNullOrWhiteSpace(_smtpUsername))
            {
                throw new InvalidOperationException("EmailSettings:SmtpUsername is not configured");
            }
            if (string.IsNullOrWhiteSpace(_smtpPassword))
            {
                throw new InvalidOperationException("EmailSettings:SmtpPassword is not configured");
            }
            if (string.IsNullOrWhiteSpace(_fromEmail))
            {
                throw new InvalidOperationException("EmailSettings:FromEmail is not configured");
            }
        }

        public async Task SendPasswordResetEmailAsync(string toEmail, string resetLink, string userName = "")
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_fromName, _fromEmail));
                message.To.Add(new MailboxAddress(userName, toEmail));
                message.Subject = "Reset Your Password - Tide of Destiny";

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body {{
            font-family: Arial, sans-serif;
            line-height: 1.6;
            color: #333;
            max-width: 600px;
            margin: 0 auto;
            padding: 20px;
        }}
        .container {{
            background-color: #f9f9f9;
            padding: 30px;
            border-radius: 10px;
            border: 1px solid #ddd;
        }}
        .header {{
            text-align: center;
            margin-bottom: 30px;
        }}
        .header h1 {{
            color: #2c3e50;
            margin: 0;
        }}
        .content {{
            background-color: white;
            padding: 20px;
            border-radius: 5px;
            margin-bottom: 20px;
        }}
        .button {{
            display: inline-block;
            padding: 12px 30px;
            background-color: #3498db;
            color: white !important;
            text-decoration: none;
            border-radius: 5px;
            margin: 20px 0;
            font-weight: bold;
        }}
        .button:hover {{
            background-color: #2980b9;
        }}
        .footer {{
            text-align: center;
            color: #7f8c8d;
            font-size: 12px;
            margin-top: 20px;
        }}
        .warning {{
            color: #e74c3c;
            font-size: 14px;
            margin-top: 20px;
            padding: 10px;
            background-color: #fff3cd;
            border-left: 4px solid #ffc107;
            border-radius: 4px;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>🌊 Tide of Destiny</h1>
        </div>
        <div class='content'>
            <p>Hello{(string.IsNullOrEmpty(userName) ? "" : $" {userName}")},</p>
            <p>We received a request to reset your password for your Tide of Destiny account.</p>
            <p>Click the button below to reset your password:</p>
            <div style='text-align: center;'>
                <a href='{resetLink}' class='button'>Reset Password</a>
            </div>
            <p>Or copy and paste this link into your browser:</p>
            <p style='word-break: break-all; color: #3498db;'>{resetLink}</p>
            <div class='warning'>
                <strong>⚠️ Security Notice:</strong> This link will expire in 1 hour. If you did not request a password reset, please ignore this email and your password will remain unchanged.
            </div>
        </div>
        <div class='footer'>
            <p>© {DateTime.Now.Year} Tide of Destiny. All rights reserved.</p>
            <p>This is an automated email, please do not reply.</p>
        </div>
    </div>
</body>
</html>",
                    TextBody = $@"Hello{(string.IsNullOrEmpty(userName) ? "" : $" {userName}")},

We received a request to reset your password for your Tide of Destiny account.

Click the link below to reset your password:
{resetLink}

This link will expire in 1 hour.

If you did not request a password reset, please ignore this email and your password will remain unchanged.

© {DateTime.Now.Year} Tide of Destiny. All rights reserved.
This is an automated email, please do not reply."
                };

                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_smtpHost, _smtpPort, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_smtpUsername, _smtpPassword);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                // Log the error with more details
                var errorMessage = $"Failed to send email to {toEmail}. ";
                
                if (ex is MailKit.Security.AuthenticationException)
                {
                    errorMessage += "Authentication failed. Please check your SMTP username and password (App Password for Gmail).";
                }
                else if (ex.Message.Contains("Unable to connect"))
                {
                    errorMessage += $"Cannot connect to SMTP server {_smtpHost}:{_smtpPort}. Check your network connection and SMTP settings.";
                }
                else
                {
                    errorMessage += $"Error: {ex.Message}";
                }
                
                throw new Exception(errorMessage, ex);
            }
        }
    }
}

