namespace TaskManager_Discord_Bot.Tools
{
    public static class TaskManager
    {
        private static List<Models.Task> tasks = new List<Models.Task>();

        public static IReadOnlyList<Models.Task> Tasks => tasks.AsReadOnly();

        private static readonly string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tasks.txt");

        public static void AddTask(Models.Task task)
        {
            task.Id = tasks.Count > 0 ? tasks[tasks.Count - 1].Id + 1 : 1;
            tasks.Add(task);
        }

        public static void RemoveTask(Models.Task task)
        {
            tasks.Remove(task);
        }

        public static void ClearTasks()
        {
            tasks.Clear();
        }

        public static void SaveTasks()
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (Models.Task task in tasks)
                {
                    writer.WriteLine(task.ToString());
                }
            }
        }

        public static void LoadTasks()
        {
            ClearTasks();
            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split('|');
                        if (parts.Length == 5) // Adjusted length check to 5
                        {
                            string name = parts[1];
                            string description = parts[2];
                            bool isCompleted = bool.Parse(parts[3]); // Adjusted index to 3
                            bool isDaily = bool.Parse(parts[4]);     // Adjusted index to 4
                            tasks.Add(new Models.Task(name, description, isCompleted, isDaily));
                        }
                    }
                }
            }
        }
    }
}
