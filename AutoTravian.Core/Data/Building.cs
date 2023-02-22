using AutoTravian.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTravian.Core.Data
{
    public class Building
    {
        public int id { get; set; }
        public BuildingTypes Field { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public float Production { get; set; }
        public float ProductionNextLevel { get; set; }
    }
}
