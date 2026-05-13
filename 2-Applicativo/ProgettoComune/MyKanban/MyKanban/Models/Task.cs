using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyKanban.Models
{
    public class Task
    {
        public string Title { get; set; }
        public string statusTask { get; set; }
        public string priorityTask { get; set; }
        public string description { get; set; } = string.Empty;
        public string underTask { get; set; } = string.Empty;
        public DateTime deadline { get; set; }
        public string ToRiga()
        {
            return $"{Title};{statusTask};{priorityTask}; {description}; {underTask};{deadline}";
        }
        public static Task FromRiga(string riga)
        {
            string[] parts = riga.Split(';');
            return new Task
            {
                Title = parts[0].Trim(),
                statusTask = parts[1].Trim(),
                priorityTask = parts[2].Trim(),
                description = parts[3].Trim(),
                underTask = parts[4].Trim(),
                deadline = DateTime.Parse(parts[5].Trim())
            };
        }
    }  
}
