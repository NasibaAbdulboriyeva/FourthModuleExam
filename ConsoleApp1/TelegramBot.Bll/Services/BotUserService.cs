using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Dal;
using TelegramBot.Dal.Entities;

namespace TelegramBot.Bll.Services;

public class BotUserService : IBotUserService
{
    private readonly MainContext mainContext;
    public BotUserService(MainContext mainContext)
    {
        this.mainContext = mainContext;
    }
    public async Task AddUserAsync(BotUser user)
    {
        var dbUser = await mainContext.Users.FirstOrDefaultAsync(x => x.ChatId == user.ChatId);
        if (dbUser != null)
        {
            Console.WriteLine($"user with this {user.ChatId} already exists");
        }
        try
        {
            await mainContext.AddAsync(user);
            await mainContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public async Task<List<BotUser>> GetAllUsersAsync()
    {
        var users =  await mainContext.Users.ToListAsync();
        return users;
    }

    public async Task<BotUser> GetUsersByChatIdAsync(long ChatId)
    {
        var user = await mainContext.Users.FirstOrDefaultAsync(u => u.ChatId == ChatId);
        if (user == null)
        {
            Console.WriteLine("user not found");
        }
        return user;
    }

    public async Task<long> GetUserIdByChatIdAsync(long ChatId)
    {

        var botUserId = await mainContext.Users
        .Where(u => u.ChatId == ChatId)
        .Select(u => (long?)u.BotUserId)
        .FirstOrDefaultAsync();

        if (botUserId == null)
        {
            Console.WriteLine($"User with Telegram ID {ChatId} not found.");
        }

        return botUserId ?? 0;
    }

    public async Task UpdateUserAsync(BotUser user)
    {
        var dbUser = await mainContext.Users.FirstOrDefaultAsync(x => x.ChatId == user.ChatId);
        dbUser = user;
        mainContext.Update(dbUser);
        await mainContext.SaveChangesAsync();
    }
}
