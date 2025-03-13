using ExamBot.Dal;
using ExamBot.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamBot.Bll.Services;

public class UserService : IUserService
{
    private readonly MainContext mainContext;
    public UserService(MainContext mainContext)
    {
        this.mainContext = mainContext;
    }
    public async Task AddUserAsync(User user)
    {
        var dbUser = await mainContext.Users.FirstOrDefaultAsync(x => x.ChatId == user.ChatId);
        if (dbUser != null)
        {
            Console.WriteLine($"With user id:{user.ChatId} already exists");
            return;
        }
        try
        {
            await mainContext.Users.AddAsync(user);
            await mainContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public async Task DeleteUserInfoAsync(User user)
    {
        var dbUser = await mainContext.Users.FirstOrDefaultAsync(x => x.ChatId == user.ChatId);
        if (dbUser == null)
        {
            Console.WriteLine($"User with telegram id:{user.ChatId} not found");
            return;
        }
        mainContext.Users.Remove(user);
    }

    //public async Task DeleteUserAsync(User user)
    //{
    //    var dbUser = await mainContext.Users.FirstOrDefaultAsync(x => x.ChatId == user.ChatId);
    //    if (dbUser == null)
    //    {
    //        Console.WriteLine($"User with telegram id:{user.ChatId} not found");
    //        return;
    //    }
    //    mainContext.Users.Remove(user);
    //}

    public async Task<List<User>> GetAllUsersAsync()
    {
        var users = await mainContext.Users.ToListAsync();
        return users;
    }

    public async Task<User> GetBotUserByTelegramUserIdAsync(long telegramUserId)
    {
        var botUser = await mainContext.Users.FirstOrDefaultAsync(x => x.ChatId == telegramUserId);
        if (botUser == null)
        {
            Console.WriteLine($"User with telegram id:{telegramUserId} not found");
        }
        return botUser;


    }

    public async Task<long> GetBotUserIdByTelegramUserIdAsync(long telegramUserId)
    {
        var botUserId = await mainContext.Users
        .Where(u => u.ChatId == telegramUserId)
        .Select(u => (long?)u.BotUserId)
        .FirstOrDefaultAsync();

        if (botUserId == null)
        {
            Console.WriteLine($"User with Telegram ID {telegramUserId} not found.");
        }

        return botUserId ?? 0;
    }

    public async Task UpdateUserAsync(User user)
    {
        var dbUser = await mainContext.Users.FirstOrDefaultAsync(x => x.ChatId == user.ChatId);
        mainContext.Users.Update(user);
        await mainContext.SaveChangesAsync();
    }
}
