
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TaskManager_Discord_Bot.Tools;

namespace TaskManager_Discord_Bot.Commands.Slash
{
    public class DisciplineCommandsSL : ApplicationCommandModule
    {
        [SlashCommand("AddTask", "Adds Task")]
        public async System.Threading.Tasks.Task AddTask(InteractionContext itx, [Option("name","Name")] string task, [Option("desc","Desc")]string description, [Option("daily", "Daily",true)]bool IsDaily = true)
        {
            await itx.DeferAsync();
            DiscordEmbedBuilder discordEmbedBuilder = new DiscordEmbedBuilder();
            discordEmbedBuilder.Color = DiscordColor.Purple;
            discordEmbedBuilder.Title = ":star: New Task Added :star:";
            discordEmbedBuilder.AddField(":page_facing_up: Task Title", task, false);
            discordEmbedBuilder.AddField(":page_with_curl: Task Description", description, false);
            discordEmbedBuilder.WithFooter($"Is daily: {IsDaily}", "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRV3w7U-CA35bK9xUC4BRTkIbunyFRph8R9sQ&usqp=CAU");
            await itx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(discordEmbedBuilder));
            TaskManager.AddTask(new Models.Task(task, description,false, IsDaily));
            TaskManager.SaveTasks();
        }
        [SlashCommand("Tasks", "List Tasks")]
        public async System.Threading.Tasks.Task Tasks(InteractionContext itx, [Option("unfinished", "Unfinished")] bool onlyUnfinished = false)
        {
            await itx.DeferAsync();
            var embeds = new List<DiscordEmbed>();
            bool tasksFound = false;

            foreach (var task in TaskManager.Tasks)
            {
                if (onlyUnfinished && task.IsCompleted)
                    continue;

                tasksFound = true;

                DiscordEmbedBuilder discordEmbedBuilder = new DiscordEmbedBuilder();
                discordEmbedBuilder.Color = DiscordColor.Purple;
                discordEmbedBuilder.Title = task.Name;
                discordEmbedBuilder.AddField("Description", $"{task.Description}", false);
                discordEmbedBuilder.AddField("Is Daily", task.IsDaily ? ":bell:" : ":no_bell:", true);
                discordEmbedBuilder.AddField("Is Completed", task.IsCompleted ? ":green_circle:" : ":red_circle:", true);
                embeds.Add(discordEmbedBuilder);
            }
                
            if (!tasksFound)
            {
                DiscordEmbedBuilder errorEmbed = new DiscordEmbedBuilder();
                errorEmbed.Color = DiscordColor.Red;

                if (onlyUnfinished)
                {
                    errorEmbed.Title = "No Unfinished Tasks Found";
                    errorEmbed.Description = "There are no unfinished tasks.";
                }
                else
                {
                    errorEmbed.Title = "No Tasks Found";
                    errorEmbed.Description = "There are no tasks at all.";
                }

                embeds.Add(errorEmbed);
            }

            await itx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbeds(embeds));
        }

        [SlashCommand("TaskFinish", "Finishs a task")]
        public async System.Threading.Tasks.Task TaskFinish(InteractionContext itx, [Option("taskname", "Name")] string taskName)
        {
            await itx.DeferAsync();
            DiscordEmbedBuilder discordEmbedBuilder = new DiscordEmbedBuilder();
            discordEmbedBuilder.Color = DiscordColor.Purple;
            Models.Task task = TaskManager.Tasks.FirstOrDefault(t => t.Name == taskName);
            if (task != null)
            {
                discordEmbedBuilder.Title = "Task Finished";
                discordEmbedBuilder.Description = "Finished the following task:";
                if (task.IsCompleted)
                {
                    discordEmbedBuilder.Title = "Error";
                    discordEmbedBuilder.Description = "Task already complete.";
                    await itx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(discordEmbedBuilder));
                    return;
                }
                discordEmbedBuilder.AddField(task.Name,task.Description);
                task.IsCompleted = true;
                await itx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(discordEmbedBuilder));
                TaskManager.SaveTasks(); 
            }
            else
            {
                discordEmbedBuilder.Title = "Error";
                discordEmbedBuilder.Description= "Task not found.";
                await itx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(discordEmbedBuilder));
            }
        }

        [SlashCommand("TaskRemove", "Removes a task")]
        public async System.Threading.Tasks.Task TaskRemove(InteractionContext itx, [Option("taskname", "Name")] string taskName)
        {
            await itx.DeferAsync();
            DiscordEmbedBuilder discordEmbedBuilder = new DiscordEmbedBuilder();
            discordEmbedBuilder.Color = DiscordColor.Purple;
            Models.Task task = TaskManager.Tasks.FirstOrDefault(t => t.Name == taskName);
            if (task != null)
            {
                discordEmbedBuilder.Title = "Task Removed";
                discordEmbedBuilder.Description = "Removed the following task:";

                discordEmbedBuilder.AddField(task.Name, task.Description);

                await itx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(discordEmbedBuilder));
                TaskManager.RemoveTask(task);
                TaskManager.SaveTasks();
            }
            else
            {
                discordEmbedBuilder.Title = "Error";
                discordEmbedBuilder.Description = "Task not found.";
                await itx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(discordEmbedBuilder));
            }
        }
    }
}
