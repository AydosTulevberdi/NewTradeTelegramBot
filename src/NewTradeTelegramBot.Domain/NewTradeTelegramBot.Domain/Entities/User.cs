namespace NewTradeTelegramBot.Domain.Entities;

public class User
{
    public long ChatId { get; set; }
    public string? UserName { get; set; }
    public string? PhoneNumber { get; set; }
    public string Role { get; set; } = "User";
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
}
