namespace TideOfDestiniy.BLL.DTOs.Responses
{
    public class PasswordResetResultDTO
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Token { get; set; } // Optional token for testing purposes
    }
}
