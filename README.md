# Task Manager Discord Bot

## Description
This is a Task Manager Discord Bot developed in C# using the DSharpPlus Library. The bot helps manage tasks via Discord commands and will mention you each hour to finish your tasks as long as the bot is running. Tasks reset every 24 hours. Note that you need to provide a Discord User ID as the bot will tag that user every 24 hours, and a Discord Channel ID where the bot will regularly send the unfinished tasks.

## Features
- Add, remove, and list tasks
- Automatically reset tasks every 24 hours
- Tasks that are not daily will not be reset
- Automatically remind you to complete your unfinished tasks every hour

## Setup Instructions
1. Clone the repository.
2. Open the project in Visual Studio.
3. Restore NuGet packages.
4. Configure `config.json` with your bot token and other settings (when running the bot, it will ask if you want to set it up on your own).
5. Build and run the project.

## Commands
- **/AddTask [Task Name] [Description] [Daily (true by default)]**
  - Adds a task. Each task can be either daily (resets after 24 hours) or not daily (does not reset).

- **/Tasks [Unfinished (false by default)]**
  - Lists all tasks. If `Unfinished` is set to true, it will list only the unfinished tasks.

- **/FinishTask [Task Name]**
  - Marks a task as finished.

- **/RemoveTask [Task Name]**
  - Completely removes a task.
