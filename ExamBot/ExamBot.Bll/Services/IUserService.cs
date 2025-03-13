using ExamBot.Dal.Entities;

namespace ExamBot.Bll.Services;

public interface IUserService
{
    Task AddUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task DeleteUserInfoAsync(User user);
    //Task DeleteUserAsync(User user);
    Task<List<User>> GetAllUsersAsync();
    Task<User> GetBotUserByTelegramUserIdAsync(long telegramUserId);
    Task<long> GetBotUserIdByTelegramUserIdAsync(long telegramUserId);
}