using NewTradeTelegramBot.Domain.Entities;
using NewTradeTelegramBot.Domain.Interfaces;

namespace NewTradeTelegramBot.Infrastructure.Repositories;

public class InMemoryUserRepository : IUserRepository
{
    private readonly List<User> _users = new();

    public Task AddAsync(User user)
    {
        _users.Add(user);
        return Task.CompletedTask;
    }

    public Task<User?> GetByChatIdAsync(long chatId)
    {
        var user = _users.FirstOrDefault(u => u.ChatId == chatId);
        return Task.FromResult(user);
    }

    public Task<IEnumerable<User>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<User>>(_users);
    }

    public Task UpdateAsync(User user)
    {
        var existing = _users.FirstOrDefault(u => u.ChatId == user.ChatId);
        if (existing != null)
        {
            existing.UserName = user.UserName;
            existing.PhoneNumber = user.PhoneNumber;
            existing.Role = user.Role;
        }
        return Task.CompletedTask;
    }

    public Task UpdateRoleAsync(long chatId, string role)
    {
        var user = _users.FirstOrDefault(u => u.ChatId == chatId);
        if (user != null)
            user.Role = role;
        return Task.CompletedTask;
    }
}
