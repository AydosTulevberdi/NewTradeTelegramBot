using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using NewTradeTelegramBot.Application.Services;

namespace NewTradeTelegramBot.Infrastructure.Services;

public class TelegramService : ITelegramService
{
    private readonly ITelegramBotClient _botClient;

    public TelegramService(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task SendText(long chatId, string text)
    {
        await _botClient.SendMessage(
            chatId: chatId,
            text: text,
            parseMode: ParseMode.Markdown
        );
    }

    public async Task RequestContact(long chatId)
    {
        var contactBtn = KeyboardButton.WithRequestContact("📲 Telefon yuborish");
        var markup = new ReplyKeyboardMarkup(new[] { new[] { contactBtn } })
        {
            ResizeKeyboard = true,
            OneTimeKeyboard = true
        };

        await _botClient.SendMessage(
            chatId: chatId,
            text: "Iltimos, telefon raqamingizni yuboring",
            replyMarkup: markup
        );
    }
}
