using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Logging;
using TaskManager_Discord_Bot.Commands.Slash;
using TaskManager_Discord_Bot.Models;
using TaskManager_Discord_Bot.Tools;


namespace TaskManager_Discord_Bot
{
    public class Bot
    {
        public static DateTime lastUpdateTime = new DateTime(); 
        public static DiscordClient? Client { get; private set; }
        public static InteractivityExtension? Interactivity { get; private set; }
        public static CommandsNextExtension? Commands { get; private set; }

        public async System.Threading.Tasks.Task InitializeAsync()
        {
            var config = new DiscordConfiguration
            {
                Token = Config.BotToken,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug,
            };

            Client = new DiscordClient(config);
            Client.UseInteractivity(new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromMinutes(2)
            });

            var commandsConfig = new CommandsNextConfiguration
            {
                EnableDms = false,
                EnableMentionPrefix = true,
                DmHelp = true,
                CaseSensitive = false,
                IgnoreExtraArguments = true,
            };
            var slashCommandsConfig = Client.UseSlashCommands();

            Commands = Client.UseCommandsNext(commandsConfig);

            Client.ClientErrored += DiscordClient_ClientErrored;

            slashCommandsConfig.RegisterCommands<DisciplineCommandsSL>();

            await Client.ConnectAsync();

            Timer timer = new Timer(async _ => await Update(), null, TimeSpan.Zero, TimeSpan.FromHours(2));

            await System.Threading.Tasks.Task.Delay(-1);

            timer.Dispose();
        }

        private static async System.Threading.Tasks.Task DiscordClient_ClientErrored(DiscordClient sender, ClientErrorEventArgs args)
        {
            String Message = "ClientErrord: " + args;
            Console.WriteLine(Message);

        }
        public static async System.Threading.Tasks.Task Update()
        {
            var chnlId = ulong.Parse(Config.ChannelDiscordId);
            var usrId =  ulong.Parse(Config.UserDiscordId);

            var chnl = await Bot.Client!.GetChannelAsync(chnlId);

            DiscordEmbedBuilder discordEmbedBuilder = new DiscordEmbedBuilder();
            discordEmbedBuilder.Color = DiscordColor.Purple;

            DateTime currentTime = DateTime.Now;

            // Check if it's a new day since the last update
            if (currentTime.Date > lastUpdateTime.Date)
            {
                // Reset daily tasks if it's a new day
                ResetDailyTasks();
                lastUpdateTime = currentTime;
            }

            List<Models.Task> dailyTasks = TaskManager.Tasks.Where(task => task.IsDaily).ToList();
            List<Models.Task> incompleteDailyTasks = dailyTasks.Where(task => !task.IsCompleted).ToList();

            if (incompleteDailyTasks.Count > 0)
            {
                discordEmbedBuilder.Title = "Make sure to complete the following daily tasks:";
                string sentence = string.Empty;
                for (int i = 0; i < incompleteDailyTasks.Count; i++)
                {
                    var task = incompleteDailyTasks[i];
                    sentence += $"{i + 1}. " + task.Name + "\n";
                    task.IsCompleted = false;
                }

                discordEmbedBuilder.Description = sentence;
                await chnl.SendMessageAsync(embed: discordEmbedBuilder, content: $"<@{usrId}>");
            }

            List<Models.Task> nonDailyTasks = TaskManager.Tasks.Where(task => !task.IsDaily && !task.IsCompleted).ToList();
            if (nonDailyTasks.Count > 0)
            {
                discordEmbedBuilder.Title = "Make sure to complete the following non-daily tasks:";
                string sentence = string.Empty;
                for (int i = 0; i < nonDailyTasks.Count; i++)
                {
                    var task = nonDailyTasks[i];
                    sentence += $"{i+1}. "+ task.Name + "\n";
                    task.IsCompleted = true;
                }
                discordEmbedBuilder.Description = sentence;
                await chnl.SendMessageAsync(embed: discordEmbedBuilder, content: $"<@{usrId}>");
            }
            if (currentTime.Date > lastUpdateTime.Date)
            {
                // Reset daily tasks if it's a new day
                ResetDailyTasks();
                lastUpdateTime = currentTime;
            }
            // Save tasks to update the IsCompleted attribute
            TaskManager.SaveTasks();
            SaveLastUpdateTime();

        }

        static void ResetDailyTasks()
        {
            foreach (var task in TaskManager.Tasks)
            {
                if (task.IsDaily)
                {
                    // Reset the IsCompleted attribute for daily tasks
                    task.IsCompleted = false;
                }
            }
        }

        static void SaveLastUpdateTime()
        {
            string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "timestamp.txt");
            File.WriteAllText(configFilePath, lastUpdateTime.ToString());
        }

    }

}
