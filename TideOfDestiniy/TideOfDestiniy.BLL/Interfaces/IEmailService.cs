namespace TideOfDestiniy.BLL.Interfaces
{
    public interface IEmailService
    {
        Task SendPasswordResetEmailAsync(string toEmail, string resetLink, string userName = "");
    }
}

