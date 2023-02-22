using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTravian.Core.Data
{
    public class Coordinates : IEnumerable
    {
        public string X { get; set; }
        public string Y { get; set; }

        public IEnumerator GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
