using Microsoft.Extensions.Configuration;
using NewTradeTelegramBot.Application.Services;
using NewTradeTelegramBot.Domain.Entities;
using NewTradeTelegramBot.Domain.Interfaces;
using Telegram.Bot.Types;

namespace NewTradeTelegramBot.Application.Handlers;

public class MessageHandler
{
    private readonly ITelegramService _telegramService;
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _config;

    public MessageHandler(
        ITelegramService telegramService,
        IUserRepository userRepository,
        IConfiguration config)
    {
        _telegramService = telegramService;
        _userRepository = userRepository;
        _config = config;
    }

    public async Task HandleAsync(Update update)
    {
        var msg = update.Message;

        if (msg == null) return;

        var  chatId = msg.Chat.Id;
        var username = msg.From?.Username ?? "";

        var user = await _userRepository.GetByChatIdAsync(chatId);

        // Super adminni aniqlaymiz
        var superAdminUsername = _config["SuperAdminUsername"];

        // 1. Agar bu yangi foydalanuvchi bo‘lsa – Contact so‘raymiz
        if (user == null && msg.Contact == null)
        {
            await _telegramService.RequestContact(chatId);
            return;
        }

        // 2. Agar contact yuborgan bo‘lsa – user yaratamiz
        if (msg.Contact != null)
        {
            var newUser = new Domain.Entities.User
            {
                ChatId = chatId,
                UserName = username,
                PhoneNumber = msg.Contact.PhoneNumber,
                Role = (username == superAdminUsername) ? "Admin" : "User"
            };

            await _userRepository.AddAsync(newUser);
            await _telegramService.SendText(chatId, "✅ Telefon raqamingiz saqlandi.");
            return;
        }

        // 3. Agar foydalanuvchi bazada bor bo‘lsa va admin bo‘lsa
        if (user is not null && (user.Role == "Admin" || username == superAdminUsername))
        {
            if (msg.Text == "/users")
            {
                var users = await _userRepository.GetAllAsync();
                var text = string.Join("\n", users.Select(u =>
                    $"{u.UserName ?? "(no username)"} - {u.PhoneNumber} - {u.Role}"));
                await _telegramService.SendText(chatId, text);
            }
            else
            {
                await _telegramService.SendText(chatId, $"👋 Salom, {user.Role}!");
            }
        }
        else
        {
            // Oddiy foydalanuvchi uchun
            await _telegramService.SendText(chatId, "👋 Botga hush kelibsiz.");
        }
    }
}
