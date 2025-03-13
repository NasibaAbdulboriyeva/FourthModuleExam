using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Bll.Services;
using TelegramBot.Dal.Entities;

namespace TelegramBot
{
    public class BotListenerService
    {
        private static string botToken = "8001669970:AAF6UOQ2K6IsxM-5WiNrm2HWVlrwzCtjWvs";
        private TelegramBotClient botClient = new TelegramBotClient(botToken);
        private readonly IBotUserService botUserService;

        public BotListenerService(IBotUserService userService)
        {
            this.botUserService = userService;
        }


        public async Task StartBot()
        {
            botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync
                );

            Console.WriteLine("Bot is runing");

            Console.ReadKey();

        }

        private async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellation)
        {
            var user = update.Message.Chat;
            var message = update.Message;
            var botUserId = await botUserService.GetUserIdByChatIdAsync(user.Id);

            if (update.Type == UpdateType.Message)
            {

                if (message.Text == "/start")
                {
                    var savingUser = new BotUser()
                    {
                        ChatId = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        BirthDate = DateTime.UtcNow,

                    };

                    await botUserService.AddUserAsync(savingUser);

                    await SendStartMenu(bot, user.Id);
                    return;
                }
            }

            if (message.Text == "Main menu")
            {
                var menu = new ReplyKeyboardMarkup(new[]
                {
                    new[]
                    {
                        new KeyboardButton("Fill data"),
                        new KeyboardButton("Delete data"),
                        new KeyboardButton("Get data")
{
                        } } })

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

            if (message.Text == "Get data")
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
                     "Tashkent ";

                await bot.SendTextMessageAsync(
                chatId: user.Id,
                text: userInfoText,
                parseMode: ParseMode.Markdown
                );

                return;
            }

            if (message.Text.StartsWith("Get data"))
            {
                var userInfotext = message.Text;
                var data = userInfotext.Split("\n");
                var userInfo = new BotUser()
                {
                    FirstName = data[1].Trim(),
                    LastName = data[2].Trim(),
                    Email = data[3].Trim(),
                    PhoneNumber = data[4].Trim(),
                    Address = data[5].Trim(),
                    BotUserId = botUserId
                };

               


                var textToBotUser = "";

                if (userInfo.PhoneNumber==null)
                {
                    textToBotUser = "please enter your phone number";
                }
                else
                {
                    textToBotUser += userInfo;
                }

                await bot.SendTextMessageAsync(
                chatId: user.Id,
                text: textToBotUser,
                parseMode: ParseMode.Markdown
                );

                await SendStartMenu(bot, user.Id);
            }



        }
        private async Task HandleErrorAsync(ITelegramBotClient bot, Exception exception, CancellationToken cancellation)
        {
            Console.WriteLine(exception.Message);
        }

        private static async Task SendStartMenu(ITelegramBotClient botClient, long userId)
        {

            var menu = new ReplyKeyboardMarkup(
                new KeyboardButton("Fill data "),
                new KeyboardButton("Update data"),
                new KeyboardButton("Delete data"))
            {
                ResizeKeyboard = true
            };




            var introText = @"
            🌟 *Welcome to the TelegramBot 😊 ";

            await botClient.SendTextMessageAsync(
                chatId: userId,
                text: introText,
                parseMode: ParseMode.Markdown,
                replyMarkup: menu
            );
        }
    }

}
