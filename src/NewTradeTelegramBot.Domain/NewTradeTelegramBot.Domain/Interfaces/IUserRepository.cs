using NewTradeTelegramBot.Domain.Entities;

namespace NewTradeTelegramBot.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByChatIdAsync(long chatId);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task UpdateRoleAsync(long chatId, string role);
    Task<IEnumerable<User>> GetAllAsync();
}
