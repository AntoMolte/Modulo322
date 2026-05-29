using Microsoft.Maui.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyKanban.Models
{
    public class KanbanTask
    {
        private static int _nextId = 1;

        public int ID { get; set; }

        public string Title { get; set; }
        public string statusTask { get; set; }
        public string priorityTask { get; set; }
        public string description { get; set; } = string.Empty;
        public string underTask { get; set; } = string.Empty;

        public DateTime deadline { get; set; }

        public DateTime DueDate => deadline;

        public KanbanTask()
        {
            ID = _nextId++;
        }

        public string ToRiga()
        {
            return $"{ID};{Title};{statusTask};{priorityTask};{description};{underTask};{deadline}";
        }

        public static KanbanTask FromRiga(string riga)
        {
            string[] parts = riga.Split(';');

            var task = new KanbanTask
            {
                ID = int.Parse(parts[0].Trim()),
                Title = parts[1].Trim(),
                statusTask = parts[2].Trim(),
                priorityTask = parts[3].Trim(),
                description = parts[4].Trim(),
                underTask = parts[5].Trim(),
                deadline = DateTime.Parse(parts[6].Trim())
            };

            if (task.ID >= _nextId)
                _nextId = task.ID + 1;

            return task;
        }
    }
}