using AutoTravian.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTravian.UI.Models.Entities
{
    [System.Reflection.ObfuscationAttribute(Feature = "renaming", ApplyToMembers = true)]
    public class UserEntity
    {
        public int id { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public bool Remember { get; set; }
        public DateTime LastLogin { get; set; }
    }
}
