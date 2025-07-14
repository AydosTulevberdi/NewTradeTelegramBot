namespace NewTradeTelegramBot.Application.Services;

public interface ITelegramService
{
    Task SendText(long chatId, string text);
    Task RequestContact(long chatId);
}
