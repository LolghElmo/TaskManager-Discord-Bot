using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager_Discord_Bot.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsDaily { get; set; }
        public Task( string name, string description, bool isCompleted = false, bool isDaily = true)
        {
            Name = name;
            Description = description;
            IsCompleted = isCompleted;
            IsDaily = isDaily;
        }

        public override string ToString()
        {
            return $"{Id}|{Name}|{Description}|{IsCompleted}|{IsDaily}";
        }
    }
}
