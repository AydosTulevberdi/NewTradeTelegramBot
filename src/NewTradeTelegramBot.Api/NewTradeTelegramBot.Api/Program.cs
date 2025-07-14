using NewTradeTelegramBot.Application.Handlers;
using NewTradeTelegramBot.Application.Services;
using NewTradeTelegramBot.Domain.Interfaces;
using NewTradeTelegramBot.Infrastructure.Repositories;
using NewTradeTelegramBot.Infrastructure.Services;
using Telegram.Bot;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;

var builder = WebApplication.CreateBuilder(args);

// 🔐 Token va admin username’ni olish
var token = builder.Configuration["TelegramToken"]
            ?? throw new InvalidOperationException("❗ TelegramToken environment variable not set");
var adminUsername = builder.Configuration["SuperAdminUsername"]
                    ?? throw new InvalidOperationException("❗ SuperAdminUsername environment variable not set");

// 🧩 DI servislari
builder.Services.AddSingleton<ITelegramBotClient>(_ =>
    new TelegramBotClient(token));
builder.Services.AddSingleton<ITelegramService, TelegramService>();
builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();
builder.Services.AddSingleton<MessageHandler>();

// 🌐 JSON formatlash uchun Newtonsoft.Json
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        // Misol uchun: null qiymatlarni e’tiborsiz qoldirish
        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
        // Contract resolver: default
        options.SerializerSettings.ContractResolver =
            new Newtonsoft.Json.Serialization.DefaultContractResolver();
    });

var app = builder.Build();

app.UseHttpsRedirection();

// 🛰 Webhook endpoint
app.MapPost("/webhook", async (Telegram.Bot.Types.Update update, MessageHandler handler) =>
{
    await handler.HandleAsync(update);
    return Results.Ok();
});

app.Run();
