using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using System.IO;
using System.Net;


namespace MVC_EF_BOT.Classes
{
    public class TeleBot
    {
        static TelegramBotClient bot;
        static InlineKeyboardMarkup inkeyMrk;
        static ReplyKeyboardMarkup rkm;
        const string usage = @"دستورات:
            سلام/درود
            خوبی
            چطوری /چه طوری            
            فیلم
            تبلیغ / تبلیغات
            پیام همگانی

            ";
        static MVC_EF_BOT.Controllers.BotUsersController botCntrlr;
        static MVC_EF_BOT.Models.BotUser botUser;
        static List<MVC_EF_BOT.Models.BotUser> advUsersList;
        public TeleBot()
            {
            //Console.WriteLine("Bot is initializing...");
            bot = new TelegramBotClient("519136861:AAERMYATxZxEke91a6yvpjLwEOxJ2YaCWGc");
            botCntrlr = new MVC_EF_BOT.Controllers.BotUsersController();
            botUser = new MVC_EF_BOT.Models.BotUser();
            bot.OnMessage += Bot_OnMessage;
            bot.OnCallbackQuery += Bot_OnCallbackQuery;

            //Console.WriteLine("Bot is initializing Finished");
            //Console.Title = me.Username;
            bot.StartReceiving();
            //Console.ReadLine();
        }
        private static void Bot_OnCallbackQuery(object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            if (e.CallbackQuery.Data == "genre-family")
            {
                bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "انتخاب شما خانوادگی");
                return;
            }
            else if (e.CallbackQuery.Data == "genre-animation")
            {
                bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "انتخاب شما انیمیشن");
                return;
            }
            else if (e.CallbackQuery.Data == "genre-cancel")
            {
                bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "انتخاب شما هیچی!");
                return;
            }
            else if (e.CallbackQuery.Data == "Recieve_Adv")
            {
                botUser.teleID = e.CallbackQuery.Message.Chat.Id;
                botUser.getNews = true;
                try
                {
                    botCntrlr.RegisterAdv(botUser);
                    bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "به گروه پیامگیران خوش آمدید!");
                    return;
                }
                catch(Exception ex)
                {

                }
                return;
            }
            else if (e.CallbackQuery.Data == "Cancel_Adv")
            {
                botUser.teleID = e.CallbackQuery.Message.Chat.Id;
                botUser.getNews = false;
                try
                {
                    botCntrlr.RegisterAdv(botUser);
                    bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "به سلامت. خودت با پای خودت برمیگردی! حالا ببین کی گفتم.!");
                    return;
                }
                catch (Exception ex)
                {

                }
                return;
            }


        }

        private static void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {

            if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Photo ||
                e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Document
                )
            {
                DownloadFile(e.Message);
                return;
            }          

            string msg = e.Message.Text;
            if (msg == null) return;
            
            botUser.teleID = e.Message.Chat.Id;
            botUser.getNews = false;
            try
            {
                botCntrlr.Register(botUser);
            }
            catch(Exception ex)
            {
                throw (ex);
            }
            
            

            
            string msgGist = new string(msg.Where(c => !char.IsPunctuation(c)).ToArray());

            //Console.WriteLine(e.Message.Chat.Id + "has sent a message!");
            

            if (msg.Trim() == "/start" || msgGist.Contains("start"))
            {
                bot.SendTextMessageAsync(e.Message.Chat.Id, usage);
                return;
            }
            #region greeting
            if (msgGist.Trim() == "سلام" || msgGist.Trim() == "درود")
            {
                bot.SendTextMessageAsync(e.Message.Chat.Id, "درود!");
                return;
            }
            if (msgGist.Trim() == "خوبی" || msgGist.Trim() == "چطوری" || msgGist.Trim() == "چطورین" || msgGist.Trim() == "چه طوری" || msgGist.Trim() == "چه طورین")
            {
                Random r = new Random();
                int a = r.Next(0, 3);
                switch (a)
                {
                    case 0: bot.SendTextMessageAsync(e.Message.Chat.Id, "ممنونم."); break;
                    case 1: bot.SendTextMessageAsync(e.Message.Chat.Id, "مرسی، تو خوبی؟"); break;
                    case 2: bot.SendTextMessageAsync(e.Message.Chat.Id, "سپاس، شما چطوری؟"); break;
                    case 3: bot.SendTextMessageAsync(e.Message.Chat.Id, "دعا گوییم. شما خوبی؟"); break;
                }
                return;
            }
            #endregion
            
            #region adv
            if (msgGist.Trim() == "تبلیغات" || msgGist.Trim() == "تبلیغ"  || msg.Contains("تبلیغات") || msg.Contains("تبلیغ"))
            {
                var inBtnAdv = InlineKeyboardButton.WithCallbackData("دریافت تبلیغات", "Recieve_Adv");
                var inBtnCancelAdv = InlineKeyboardButton.WithCallbackData("لغو تبلیغات", "Cancel_Adv");
                var keyboard = new InlineKeyboardButton[]
                        {
                              inBtnAdv,
                              inBtnCancelAdv
                                 
                        };
                inkeyMrk = new InlineKeyboardMarkup(keyboard);
                bot.SendTextMessageAsync(e.Message.Chat.Id, "آیا مایل به دریافت تبلیغات ما هستید؟", Telegram.Bot.Types.Enums.ParseMode.Default, false, false, 0, inkeyMrk);
                return;
            }
            if (msgGist.Trim() == "پیام همگانی" || msg.Contains("پیام همگانی"))
            {
                //string msg = { ("این یک پیام همگانی و تبلیغاتی است") };
                SendMessageToAll("این یک پیام همگانی و تبلیغاتی است");
                return;
            }
            #endregion

            #region film
            if (msgGist.Trim().Contains("فیلم") || msgGist.Trim().Contains("سریال"))
            {


                //keyborad 
                rkm = new ReplyKeyboardMarkup();
                rkm.Keyboard =
                    new KeyboardButton[][]
                    {
                        new KeyboardButton[]
                    {
                        new KeyboardButton("خانوادگی اجتماعی"),
                        new KeyboardButton("انیمیشن"),
                    },

                   new KeyboardButton[]
                    {
                        new KeyboardButton("تریلر - اکشن")
                    },

                   new KeyboardButton[]
                    {
                        new KeyboardButton("کمدی"),
                        new KeyboardButton("درام"),
                        new KeyboardButton("دیگر...")
                    }
                    };

                /*
                            //keyboard2
                            var inBtnFamily = InlineKeyboardButton.WithCallbackData("خانوادگی", "genre-family");
                            var inBtnAnimation = InlineKeyboardButton.WithCallbackData("انیمیشن", "genre-animation");
                            var inBtnCancel = InlineKeyboardButton.WithCallbackData("هیچکدام", "genre-cancel");
                            var keyboard = new InlineKeyboardButton[][]
                                    {
                                        new InlineKeyboardButton[]
                                        {
                                            inBtnFamily,
                                            inBtnAnimation
                                        },
                                        new InlineKeyboardButton[]
                                        {
                                            inBtnCancel
                                        }
                                    };
                            inkeyMrk = new InlineKeyboardMarkup(keyboard);
                            */

                bot.SendTextMessageAsync(e.Message.Chat.Id, "چه فیلمی دوست دارید؟", Telegram.Bot.Types.Enums.ParseMode.Default, false, false, 0, rkm);
                return;
            }
            if (msgGist.Trim() == "انیمیشن")
            {
                bot.SendTextMessageAsync(e.Message.Chat.Id, "بچه شدی؟");
                bot.SendTextMessageAsync(
                        e.Message.Chat.Id,
                        "انتخاب شما: انیمیشن",
                        replyMarkup: new ReplyKeyboardRemove());
                return;
            }

            if (msgGist.Trim() == "خانوادگی اجتماعی")
            {
                bot.SendTextMessageAsync(e.Message.Chat.Id, "زن ذلیل، میخوای بشینی با زن ت فیلم ببینی!!");
                bot.SendTextMessageAsync(
                        e.Message.Chat.Id,
                        "انتخاب شما: خانوادگی اجتماعی",
                        replyMarkup: new ReplyKeyboardRemove());
                return;
            }
            #endregion

            if (msg.Trim()== "عکس" || msgGist.Contains("عکس"))
            {
                string fileName = @"D:\Hadi\Files\sir.png";
                Stream strm = new FileStream(fileName, FileMode.Open);
                Telegram.Bot.Types.InputFiles.InputOnlineFile photo = new Telegram.Bot.Types.InputFiles.InputOnlineFile(strm);
                //Telegram.Bot.Types.InputFiles.InputFileStream
                bot.SendPhotoAsync(e.Message.Chat.Id, photo);
            }
            //else
            //{
            // bot.SendTextMessageAsync(e.Message.Chat.Id, "متوجه نشدم");
            //}

        }
        private static void SendMessageToAll(string message)
        {
            advUsersList = botCntrlr.SelectUsersForAdv();
            foreach ( var item in advUsersList)
            {
                MVC_EF_BOT.Models.BotUser bu = item as MVC_EF_BOT.Models.BotUser;
                bot.SendTextMessageAsync(bu.teleID, message);
            }
        }
        
        public static async void DownloadFile(Message message)
        {
            Telegram.Bot.Types.File file = new Telegram.Bot.Types.File();
            if (message.Type == Telegram.Bot.Types.Enums.MessageType.Document)
            {
                file = await bot.GetFileAsync(message.Document.FileId);
            }
            else if (message.Type == Telegram.Bot.Types.Enums.MessageType.Photo)
            {
                file = await bot.GetFileAsync(message.Photo.LastOrDefault().FileId);
            }

            var file_ext = file.FilePath.Split('.').Last();
            var fileNameAndExt = file.FilePath.Split(Path.DirectorySeparatorChar).Last();

            string fileLocalName = @"D:\Hadi\Files\" + fileNameAndExt;
            Uri fileUrl = new Uri(@"https://api.telegram.org/file/bot519136861:AAERMYATxZxEke91a6yvpjLwEOxJ2YaCWGc/" + file.FilePath);
            HttpWebResponse response = null;
            WebClient wbClnt = null;
            var request = (HttpWebRequest)WebRequest.Create(fileUrl);
            request.Method = "HEAD";
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                wbClnt = new WebClient();
                wbClnt.DownloadFileAsync(fileUrl, fileLocalName);
            }
            catch (WebException ex)
            {
                /* A WebException will be thrown if the status of the response is not `200 OK` */
            }
            finally
            {
                // Don't forget to close your response.
                if (response != null)
                {
                    response.Close();
                }
                if (wbClnt != null)
                {
                    wbClnt = null;
                }
            }
            //Stream strm = new FileStream(filePath, FileMode.Open);
            //https://api.telegram.org/file/bot<token>/<file_path>

            //await bot.DownloadFileAsync(fileName, strm);
            //cancellationToken:new System.Threading.CancellationToken(false) );
            //bot.DownloadFileAsync();

        }
    }
}