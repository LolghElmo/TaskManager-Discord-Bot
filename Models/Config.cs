using Newtonsoft.Json;

namespace TaskManager_Discord_Bot.Models
{
    public class Config
    {
        private static string ConfigFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config.json");

        public static string BotToken { get; private set; }
        public static string UserDiscordId { get; private set; }
        public static string ChannelDiscordId { get; private set; }

        static Config()
        {
            LoadConfig();
        }

        private static void LoadConfig()
        {
            if (File.Exists(ConfigFilePath))
            {
                var json = File.ReadAllText(ConfigFilePath);
                var configData = JsonConvert.DeserializeObject<ConfigData>(json);

                BotToken = configData.BotToken;
                UserDiscordId = configData.UserDiscordId;
                ChannelDiscordId = configData.ChannelDiscordId;
            }
            else
            {
                throw new FileNotFoundException($"Configuration file '{ConfigFilePath}' not found.");
            }
        }

        public static void UpdateBotToken(string newBotToken)
        {
            BotToken = newBotToken;
            SaveConfig();
        }

        public static void UpdateUserDiscordId(string newUserDiscordId)
        {
            UserDiscordId = newUserDiscordId;
            SaveConfig();
        }

        public static void UpdateChannelDiscordId(string newChannelDiscordId)
        {
            ChannelDiscordId = newChannelDiscordId;
            SaveConfig();
        }

        private static void SaveConfig()
        {
            var configData = new ConfigData
            {
                BotToken = BotToken,
                UserDiscordId = UserDiscordId,
                ChannelDiscordId = ChannelDiscordId
            };

            var json = JsonConvert.SerializeObject(configData, Formatting.Indented);
            File.WriteAllText(ConfigFilePath, json);
        }

        private class ConfigData
        {
            public string BotToken { get; set; }
            public string UserDiscordId { get; set; }
            public string ChannelDiscordId { get; set; }
        }
    }
}
