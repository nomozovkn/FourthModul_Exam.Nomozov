using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using Telegram.Bot.Types;
using ExamBot.Bll.Services;
using ExamBot.Dal.Entities;

namespace ExamBot.Server;

public class BotListenerService
{
    private static string botToken = "7844391521:AAF6YU-QSIX6Nyrh1_stCJ8lxSGgCozZISM";
    private TelegramBotClient botClient = new TelegramBotClient(botToken);
    private readonly IUserService botUserService;
    private readonly IFillDataService userInfoService;
    //private readonly IEducationService educationService;
    //private readonly ISkillService skillService;
    //private readonly IExperienceService experienceService;
    //private readonly IFileService fileService;
    public BotListenerService(IUserService botUserService, IFillDataService userInfoService)
    {
        this.botUserService = botUserService;
        this.userInfoService = userInfoService;
        //this.educationService = educationService;
        //this.skillService = skillService;
        //this.experienceService = experienceService;
        //this.fileService = fileService;
    }
    public async Task StartBot()
    {
        botClient.StartReceiving(
           HandlUpdateAsync,
           HandlErrorAsync);

        Console.WriteLine("Bot started");
        Console.ReadLine();
    }
    public async Task HandlUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.Message)
        {
            var user = update.Message.Chat;
            var message = update.Message;
            var botUserId = await botUserService.GetBotUserIdByTelegramUserIdAsync(user.Id);
            var userInfoId = await userInfoService.GetUserInfoIdByBotUserIdAsync(botUserId);

            if (message.Text == "/start")
            {
                var savingUser = new Dal.Entities.User()
                {
                    ChatId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,


                };
                await botUserService.AddUserAsync(savingUser);

                await SendStartMenu(bot, user.Id);
                return;
            }
            if (message.Text == "/start")
            {
                var menu = new ReplyKeyboardMarkup(new[]
                {
                    new[]
                    {
                        new KeyboardButton("Fill data"),
                        new KeyboardButton("Get data"),
                        new KeyboardButton("Delete data")
                    }

                })
                {
                    ResizeKeyboard = true
                };

                await botClient.SendTextMessageAsync(
                    chatId: user.Id,
                    text: "You get main menu",
                    parseMode: ParseMode.Markdown,
                    replyMarkup: menu
                );

                return;
            }
            if (message.Text == "Fill data")
            {
                var userInfo = await userInfoService.GetUserInfoByBotUserIdAsync(botUserId);

                var menu = new ReplyKeyboardMarkup()
                {
                    ResizeKeyboard = true
                };

                var textOfUserInfo = "";

                if (userInfo == null)
                {
                    textOfUserInfo = "Your info not found press\nCreate user info button to create";

                    menu.AddButtons(
                        new KeyboardButton("Create user info"),
                        new KeyboardButton("Main menu"));
                }
                else
                {
                    textOfUserInfo = $"Your personal info below\n" +
                                     $"UserId      : {userInfo.FillDataId}\n" +
                                     $"Firstname   : {userInfo.FirstName}\n" +
                                     $"Lastname    : {userInfo.LastName}\n" +
                                     $"Phonenumber : {userInfo.PhoneNumber}\n" +
                                     $"Email       : {userInfo.Email}\n" +
                                     $"Address     : {userInfo.Address}\n";


                    menu.AddButtons(


                        new KeyboardButton("Main menu"));
                }

                await bot.SendTextMessageAsync(
                chatId: user.Id,
                text: textOfUserInfo,
                parseMode: ParseMode.Markdown,
                replyMarkup: menu
                );

                return;
            }
            if (message.Text == "Create user info")
            {
                var userInfoText = "Please enter your details in the following format:\n\n" +
                      "*First Name*\n" +
                      "*Last Name*\n" +
                      "*Email*\n" +
                      "*Phone Number*\n" +
                      "*Address*\n" +

                      "Example:\n" +
                      "John\n" +
                      "Doe\n" +
                      "john.doe@example.com\n" +
                      "+1234567890\n" +
                      "123 Main St, City, Country\n" +
                      "I am .net developer";

                await bot.SendTextMessageAsync(
                chatId: user.Id,
                text: userInfoText,
                parseMode: ParseMode.Markdown
                );

                return;
            }
            if (message.Text.StartsWith("Create user info"))
            {
                var userInfotext = message.Text;
                var data = userInfotext.Split("\n");
                var userInfo = new FillData()
                {
                    FirstName = data[1].Trim(),
                    LastName = data[2].Trim(),
                    Email = data[3].Trim(),
                    PhoneNumber = data[4].Trim(),
                    Address = data[5].Trim(),
                    BotUserId = botUserId
                };



                var resFromAddUserInfoAsync = await userInfoService.AddUserInfoAsync(userInfo);

                var textToBotUser = "";

                if (resFromAddUserInfoAsync == 0)
                {
                    textToBotUser = "Error occuried while saving";
                }
                else
                {
                    textToBotUser = "Successfully saved";
                }

                await bot.SendTextMessageAsync(
                chatId: user.Id,
                text: textToBotUser,
                parseMode: ParseMode.Markdown
                );

                await SendStartMenu(bot, user.Id);
            }
            if (message.Text == "Get data")
            {
                var userInfo = await userInfoService.GetUserInfoByBotUserIdAsync(botUserId);

                var menu = new ReplyKeyboardMarkup()
                {
                    ResizeKeyboard = true
                };

                var textOfUserInfo = "";

                if (userInfo == null)
                {
                    textOfUserInfo = "Your info not found press\nCreate user info button to create";
                    menu.AddButtons(
                        new KeyboardButton("Create user info"),
                        new KeyboardButton("Main menu"));
                }
                else
                {
                    textOfUserInfo = $"Your personal info below\n" +
                                     $"UserId      : {userInfo.FillDataId}\n" +
                                     $"Firstname   : {userInfo.FirstName}\n" +
                                     $"Lastname    : {userInfo.LastName}\n" +
                                     $"Phonenumber : {userInfo.PhoneNumber}\n" +
                                     $"Email       : {userInfo.Email}\n" +
                                     $"Address     : {userInfo.Address}\n";

                    menu.AddButtons(
                        new KeyboardButton("Main menu"));
                }



                await bot.SendTextMessageAsync(
                    chatId: user.Id,
                    text: textOfUserInfo,
                    parseMode: ParseMode.Markdown

                    );

            }
            if (message.Text == "Delete data")
            {
                var userInfo = await userInfoService.GetUserInfoByBotUserIdAsync(botUserId);
                if (userInfo == null)
                {
                    await bot.SendTextMessageAsync(
                        chatId: user.Id,
                        text: "Your info not found",
                        parseMode: ParseMode.Markdown
                        );
                }
                else
                {
                    await userInfoService.DeleteUserInfoAsync(userInfo);
                    await bot.SendTextMessageAsync(
                        chatId: user.Id,
                        text: "Your info deleted",
                        parseMode: ParseMode.Markdown
                        );
                }

            }
            if (message.Text == "Ertaga hamma darsga kech qolmay kelsin")
            {
                var users = await botUserService.GetAllUsersAsync();
                var ids = users.Select(x => x.ChatId).ToArray();

                await bot.SendTextMessageAsync(ids, "Ertaga hamma darsga kech qolmay kelsin");
            }
        }
    }
    public async Task HandlErrorAsync(ITelegramBotClient bot, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine(exception.Message);
    }

    private static async Task SendStartMenu(ITelegramBotClient botClient, long userId)
    {
        var menu = new ReplyKeyboardMarkup(new[]
        {
            new[]
            {
                new KeyboardButton("Fill data"),
                new KeyboardButton("Get data"),
                new KeyboardButton("Delete data")
            }
            
        })
        {
            ResizeKeyboard = true
        };



        
    }
}
