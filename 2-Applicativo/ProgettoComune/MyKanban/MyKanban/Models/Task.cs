using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyKanban.Models
{
    class Task
    {
        public string Title { get; set; }
        public string statusTask { get; set; }
        public string priorityTask { get; set; }
        public string description { get; set; } = string.Empty;
        public string underTask { get; set; } = string.Empty;
        public DateTime deadline { get; set; }
    }
}
