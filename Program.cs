using TaskManager_Discord_Bot.Models;
using TaskManager_Discord_Bot.Tools;

namespace TaskManager_Discord_Bot
{
    public class Program
    {
        private static async System.Threading.Tasks.Task Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Do you wish to change your Config? (answer with yes/no)");
            Console.WriteLine("Note: If it's first time running then answer with yes.");
            Console.ForegroundColor = ConsoleColor.Red;
            string answr = Console.ReadLine()!;
            if (answr == "yes")
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Your Discord Bot Tokens: ");
                Console.ForegroundColor = ConsoleColor.Red;
                string dscrdBotTokens = Console.ReadLine()!;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Your Discord Channel ID: ");
                Console.ForegroundColor = ConsoleColor.Red;
                string chnlID = Console.ReadLine()!;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Your Discord User ID: ");
                Console.ForegroundColor = ConsoleColor.Red;
                string userID = Console.ReadLine()!;

                Config.UpdateBotToken(dscrdBotTokens);
                Config.UpdateChannelDiscordId(chnlID);
                Config.UpdateUserDiscordId(userID);
            }
            Console.ForegroundColor = ConsoleColor.White;

            TaskManager.LoadTasks();
            LoadLastUpdateTime();
            Bot bot = new Bot();
            await bot.InitializeAsync();
        }
        static void LoadLastUpdateTime()
        {
            string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "timestamp.txt");
            if (File.Exists(configFilePath))
            {
                string lastUpdateTimeString = File.ReadAllText(configFilePath);
                if (DateTime.TryParse(lastUpdateTimeString, out DateTime loadedLastUpdateTime))
                {
                    Bot.lastUpdateTime = loadedLastUpdateTime;
                }
            }
        }
    }
}