using System;
using System.ComponentModel.DataAnnotations;

namespace CTMLib.Models
{
    public class Log
    {
        private DateTime _date;

        public Log()
        {
           Id = Guid.NewGuid().ToString();
           Date=DateTime.UtcNow;
        }

        [Key]
        public string Id { get;private set; }

        [Required]
        public LogEventType EventType { get; set; }

        [Required]
        public string TableName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public DateTime Date {
            get
            {
                return _date.ToLocalTime();
            }
            set
            {
                _date = value;
            }
        }
    }

    public enum LogEventType
    {
        Add,
        Edit,
        Delete,
        
    }
}