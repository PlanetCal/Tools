using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICSToPlanetCal
{
    internal class CalendarBase
    {
        public string Source { get; set; }
        public CalendarParameters Parameters { get; set; }

        public string Location { get; set; }

        public List<EventBase> Events { get; set; } = new List<EventBase>();

        public CalendarBase(string source)
        {
            this.Source = source;
        }
    }
}
