using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoTravian.UI.Models.Entities
{
    [System.Reflection.ObfuscationAttribute(Feature = "renaming", ApplyToMembers = true)]
    internal class TaskEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Action { get; set; } = "";
        public string? Arguments { get; set; }
        public bool Active { get; set; }
        public TimeSpan Interval { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}