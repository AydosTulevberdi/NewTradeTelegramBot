using NewTradeTelegramBot.Application.Handlers;
using NewTradeTelegramBot.Application.Services;
using NewTradeTelegramBot.Domain.Interfaces;
using NewTradeTelegramBot.Infrastructure.Repositories;
using NewTradeTelegramBot.Infrastructure.Services;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);

// 🔐 Telegram tokenini olish va validatsiya
var token = builder.Configuration["TelegramToken"]
            ?? throw new InvalidOperationException("❗ TelegramToken environment variable not set");
var adminUsername = builder.Configuration["SuperAdminUsername"]
                    ?? throw new InvalidOperationException("❗ SuperAdminUsername environment variable not set");

// DI orqali servislar
builder.Services.AddSingleton<ITelegramBotClient>(_ =>
    new TelegramBotClient(token));
builder.Services.AddSingleton<ITelegramService, TelegramService>();
builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();
builder.Services.AddSingleton<MessageHandler>();

// ⚙️ Agar JSON konfiguratsiyada o‘zgarish kerak bo‘lsa:
builder.Services.AddControllers().AddNewtonsoftJson(); // bu endi aniqlanishi kerak
var app = builder.Build();

app.UseHttpsRedirection();

app.MapPost("/webhook", async (Telegram.Bot.Types.Update update, MessageHandler handler) =>
{
    await handler.HandleAsync(update);
    return Results.Ok();
});

app.Run();
