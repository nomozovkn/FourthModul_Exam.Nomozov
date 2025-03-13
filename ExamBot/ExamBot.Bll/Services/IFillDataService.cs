using ExamBot.Dal.Entities;

namespace ExamBot.Bll.Services;

public interface IFillDataService
{
    Task<long> AddUserInfoAsync(FillData userInfo);
    Task UpdateUserInfoAsync(FillData userInfo);
    Task DeleteUserInfoAsync(FillData userInfo);
    Task<long> GetUserInfoIdByBotUserIdAsync(long botUserId);
    Task<FillData> GetUserInfoByBotUserIdAsync(long botUserId);
}