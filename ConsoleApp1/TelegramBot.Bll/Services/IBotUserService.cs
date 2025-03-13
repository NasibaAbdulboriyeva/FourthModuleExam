using TelegramBot.Dal.Entities;

namespace TelegramBot.Bll.Services;

public interface IBotUserService
{
    Task AddUserAsync(BotUser user);
    Task UpdateUserAsync(BotUser user);
    Task<List<BotUser>> GetAllUsersAsync();
    Task<BotUser> GetUsersByChatIdAsync(long ChatId);
    Task<long> GetUserIdByChatIdAsync(long ChatId);

}