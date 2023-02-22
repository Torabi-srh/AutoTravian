using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTravian.Core.Util
{
    public class BuildQueue
    {
        public int Location { get; set; }
        public int ToLevel { get; set; }
        public int Id { get; set; }

        public BuildQueue(int location, int tolevel, int id)
        {
            Location = location;
            ToLevel = tolevel;
            Id = id;
        }
    }
}
