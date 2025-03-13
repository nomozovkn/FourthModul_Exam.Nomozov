using ExamBot.Dal;
using ExamBot.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamBot.Bll.Services;

public class FillDataService : IFillDataService
{
    private readonly MainContext mainContext;
    public FillDataService(MainContext mainContext)
    {
        this.mainContext = mainContext;
    }
    public async Task<long> AddUserInfoAsync(FillData userInfo)
    {
        try
        {
            var dbUserInfo = await mainContext.FillDatas.AddAsync(userInfo);
            await mainContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return 0l;
        }
        return userInfo.FillDataId;
    }

    public async Task DeleteUserInfoAsync(FillData userInfo)
    {
        var dbUserInfo = await mainContext.FillDatas.FirstOrDefaultAsync(ui => ui.BotUserId == userInfo.BotUserId);
        if (dbUserInfo == null)
        {
            Console.WriteLine($"User with telegram id:{userInfo.BotUserId} not found");
            return;
        }
        mainContext.FillDatas.Remove(userInfo);
    }

    public async Task<FillData> GetUserInfoByBotUserIdAsync(long botUserId)
    {
        var dbUserInfo = await mainContext.FillDatas.FirstOrDefaultAsync(ui => ui.BotUserId == botUserId);
        return dbUserInfo;
    }

    public async Task<long> GetUserInfoIdByBotUserIdAsync(long botUserId)
    {
        var dbUserInfo = await mainContext.FillDatas.FirstOrDefaultAsync(ui => ui.BotUserId == botUserId);
        if (dbUserInfo == null)
        {
            return 0l;
        }

        return dbUserInfo.FillDataId;


    }
    public async Task UpdateUserInfoAsync(FillData userInfo)
    {
        var dbUser = await mainContext.FillDatas.FirstOrDefaultAsync(x => x.BotUserId == userInfo.BotUserId);
        if (dbUser != null)
        {
            dbUser.FirstName = userInfo.FirstName;
            dbUser.LastName = userInfo.LastName;
            dbUser.Email = userInfo.Email;
            dbUser.PhoneNumber = userInfo.PhoneNumber;
            dbUser.Address = userInfo.Address;
           
            await mainContext.SaveChangesAsync();
        }
    }
}
